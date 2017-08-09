using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MatchMaker.Shared.Accounts;
using MatchMaker.Shared.Common;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Databases.Repositories.MatchDays
{
    public class MatchDayRepository : BaseRepository, IMatchDayRepository
    {
        public MatchDayRepository(IDbTransaction transaction) 
            : base(transaction)
        {
        }
        
        public async Task<MatchDay> CreateNewAsync(DateTime when, List<int> participantAccountIds, CancellationToken token)
        {
            Guard.NotInvalidDateTime(when, nameof(when));
            Guard.NotNullOrEmpty(participantAccountIds, nameof(participantAccountIds));

            var id = await this.InsertIntoMatchDaysAsync(when, token);
            await this.InsertIntoMatchDayParticipantsAsync(id, participantAccountIds, token);

            return (await this.GetMatchDaysAsync(new List<int> {id}, token)).First();
        }

        #region SQL
        private async Task<List<MatchDay>> GetMatchDaysAsync(List<int> matchDayIds, CancellationToken token)
        {
            const string sql = "SELECT MD.Id, MD.[When] " +
                               "FROM dbo.MatchDays MD " +
                               "WHERE MD.Id IN (@MatchDayIds);" +
                               
                               "SELECT MDP.MatchDayId, ParticipantId = MDP.Id, AccountId = A.Id, A.EmailAddress " +
                               "FROM dbo.MatchDayParticipants MDP " +
                               "INNER JOIN dbo.Accounts A ON MDP.AccountId = A.Id " +
                               "WHERE MDP.MatchDayId IN (@MatchDayIds);" +

                               "SELECT M.MatchDayId, Count = COUNT(*) " +
                               "FROM dbo.Matches M " +
                               "WHERE M.MatchDayId IN (@MatchDayIds) " +
                               "GROUP BY M.MatchDayId";
            using (var multi = await this.Connection.QueryMultipleAsync(this.AsCommand(sql, new {MatchDayIds = matchDayIds}, token)))
            {
                var matchDays = multi.Read<MatchDay>().ToList();
                var participants = multi.Read().ToList();
                var counts = multi.Read().ToList();

                foreach (var day in matchDays)
                {
                    day.Participants = participants
                        .Where(f => f.MatchDayId == day.Id)
                        .Select(f => new MatchDayParticipant
                        {
                            Id = f.ParticipantId,
                            Account = new Account
                            {
                                Id = f.AccountId,
                                EmailAddress = f.EmailAddress
                            }
                        })
                        .ToList();
                    day.MatchCount = counts.FirstOrDefault(f => f.MatchDayId == day.Id)?.Count ?? 0;
                }

                return matchDays;
            }
        }

        private Task<int> InsertIntoMatchDaysAsync(DateTime when, CancellationToken token)
        {
            const string sql = "INSERT INTO dbo.MatchDays ([When]) VALUES(@When); " +
                               "SELECT SCOPE_IDENTITY();";
            return this.Connection.ExecuteScalarAsync<int>(this.AsCommand(sql, new {When = when}, token));
        }

        private Task InsertIntoMatchDayParticipantsAsync(int matchDayId, List<int> participantAccountIds, CancellationToken token)
        {
            const string sql = "INSERT INTO dbo.MatchDayParticipants (MatchDayId, AccountId) " +
                               "VALUES (@MatchDayId, @AccountId)";
            return this.Connection.ExecuteAsync(this.AsCommand(sql, participantAccountIds.Select(f => new {MatchDayId = matchDayId, AccountId = f}), token));
        }
        #endregion
    }
}