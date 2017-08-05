using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Database
{
    public static class MatchDayQueries
    {
        public static async Task<int> CreateMatchDay(this IDbConnection self, DateTime when, List<int> participantIds, IDbTransaction transaction, CancellationToken cancellationToken = default(CancellationToken))
        {
            var sql = "INSERT INTO dbo.MatchDays([When]) VALUES (@When); SELECT SCOPE_IDENTITY();";
            var parameters = new
            {
                When = when
            };
            var def = new CommandDefinition(sql, parameters, cancellationToken:cancellationToken, transaction:transaction);
            var matchDayId = await self.ExecuteScalarAsync<int>(def);


            sql = "INSERT INTO dbo.MatchDayParticipants(MatchDayId, AccountId) VALUES (@MatchDayId, @AccountId);";
            var parameters2 = participantIds.Select(f => new { MatchDayId = matchDayId, AccountId = f }).ToList();
            def = new CommandDefinition(sql, parameters2, cancellationToken:cancellationToken, transaction:transaction);
            await self.ExecuteAsync(def);

            return matchDayId;
        }

        public static async Task<MatchDayCompact> GetMatchDayCompact(this IDbConnection self, int matchDayId, IDbTransaction transaction, CancellationToken cancellationToken = default(CancellationToken))
        {
            var sql = "SELECT Id, [When] FROM dbo.MatchDays WHERE Id = @Id";
            var parameters = new
            {
                Id = matchDayId
            };
            var def = new CommandDefinition(sql, parameters, cancellationToken:cancellationToken, transaction:transaction);
            var query1Result = (dynamic)await self.QueryFirstOrDefaultAsync(typeof(object), def);

            var result = new MatchDayCompact
            {
                Id = query1Result.Id,
                When = query1Result.When,
            };
            
            sql = @"SELECT A.Id, A.EmailAddress
                    FROM dbo.MatchDayParticipants MDP
                    INNER JOIN dbo.Accounts A ON A.Id = MDP.AccountId
                    WHERE MDP.MatchDayId = @Id";
            def = new CommandDefinition(sql, parameters, cancellationToken:cancellationToken, transaction:transaction);
            var query2Result = await self.QueryAsync<dynamic>(def);

            result.Participants = query2Result
                .Select(f => new AccountCompact
                {
                    Id = f.Id,
                    EmailAddress = f.EmailAddress
                })
                .ToList();

            sql = @"SELECT COUNT(*)
                    FROM dbo.Match M
                    WHERE M.MatchDayId = @Id";
            def = new CommandDefinition(sql, parameters, cancellationToken:cancellationToken, transaction:transaction);
            result.MatchCount = await self.ExecuteScalarAsync<int>(def);

            return result;
        }

        public static async Task<List<Tuple<int, int>>> GetMatchesFromMatchDay(this IDbConnection self, int matchDayId, IDbTransaction transaction, CancellationToken cancellationToken = default(CancellationToken))
        {
            var sql = "SELECT Participant1AccountId, Participant2AccountId FROM dbo.Match WHERE MatchDayId = @MatchDayId";
            var parameters = new
            {
                MatchDayId = matchDayId
            };
            var def = new CommandDefinition(sql, parameters, cancellationToken:cancellationToken, transaction:transaction);
            var result = await self.QueryAsync<dynamic>(def);

            return result
                .Select(f => Tuple.Create((int) f.Participant1AccountId, (int) f.Participant2AccountId))
                .ToList();
        }

        public static async Task<List<int>> GetParticipantsFromMatchDay(this IDbConnection self, int matchDayId, IDbTransaction transaction, CancellationToken cancellationToken = default(CancellationToken))
        {
            var sql = "SELECT AccountId FROM dbo.MatchDayParticipants WHERE MatchDayId = @MatchDayId";
            var parameters = new
            {
                MatchDayId = matchDayId
            };
            var def = new CommandDefinition(sql, parameters, cancellationToken:cancellationToken, transaction:transaction);
            return (await self.QueryAsync<int>(def)).ToList();
        }

        public static async Task<int> GetNextMatchNumberForMatchDay(this IDbConnection self, int matchDayId, IDbTransaction transaction, CancellationToken cancellationToken = default(CancellationToken))
        {
            var sql = "SELECT ISNULL(MAX(Number), 0) + 1 FROM dbo.Match WHERE MatchDayId = @MatchDayId";
            var parameters = new
            {
                MatchDayId = matchDayId,
            };
            var def = new CommandDefinition(sql, parameters, cancellationToken:cancellationToken, transaction:transaction);
            return await self.ExecuteScalarAsync<int>(def);
        }
    }
}