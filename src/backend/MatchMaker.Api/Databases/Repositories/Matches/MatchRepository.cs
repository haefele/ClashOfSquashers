using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MatchMaker.Shared.Accounts;
using MatchMaker.Shared.Common;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Databases.Repositories.Matches
{
    public class MatchRepository : BaseRepository, IMatchRepository
    {
        public MatchRepository(IDbTransaction transaction) 
            : base(transaction)
        {
        }

        public async Task<Match> GetMatchAsync(int matchId, CancellationToken token)
        {
            Guard.NotZeroOrNegative(matchId, nameof(matchId));

            return (await this.GetMatchesAsync(new List<int> {matchId}, token)).FirstOrDefault();
        }

        public async Task<List<Match>> GetMatchesAsync(int matchDayId, CancellationToken token)
        {
            Guard.NotZeroOrNegative(matchDayId, nameof(matchDayId));

            var matchIds = await this.GetMatchIdsAsync(matchDayId, token);

            if (matchIds.Any() == false)
                return new List<Match>();

            return await this.GetMatchesAsync(matchIds, token);
        }
        public async Task<Match> CreateMatchAsync(Match match, CancellationToken token)
        {
            Guard.NotNull(match, nameof(match));

            var id = await this.InsertMatchAsync(match, token);
            return (await this.GetMatchesAsync(new List<int> {id}, token)).First();
        }

        public Task UpdateMatchAsync(Match match, CancellationToken token)
        {
            Guard.NotNull(match, nameof(match));

            return this.UpdateMatchAsyncInternal(match, token);
        }

        #region SQL
        private async Task<List<int>> GetMatchIdsAsync(int matchDayId, CancellationToken token)
        {
            Guard.NotZeroOrNegative(matchDayId, nameof(matchDayId));

            const string sql = @"
SELECT M.Id
FROM dbo.Matches M
WHERE M.MatchDayId = @MatchDayId";

            return (await this.Connection.QueryAsync<int>(this.AsCommand(sql, new {MatchDayId = matchDayId}, token))).ToList();
        }
        private async Task<List<Match>> GetMatchesAsync(List<int> matchIds, CancellationToken token)
        {
            Guard.NotNullOrEmpty(matchIds, nameof(matchIds));

            const string sql = @"
SELECT M.Id, M.Number, M.MatchDayId, M.Participant1Points, M.Participant2Points, M.StartTime, M.EndTime
FROM dbo.Matches M
WHERE M.Id IN (@MatchIds);

SELECT 
    MatchId = M.Id,
	CreatedParticipantId = MDPC.Id, 
	CreatedAccountId = MDPC.AccountId, 
	CreatedEmailAddress = MDPCA.EmailAddress,
	Participant1Id = MDP1.Id,
	Participant1AccountId = MDP1.AccountId,
	Participant1EmailAddress = MDP1A.EmailAddress,
	Participant2Id = MDP2.Id,
	Participant2AccountId = MDP2.AccountId,
	Participant2EmailAddress = MDP2A.EmailAddress
FROM dbo.Matches M 
INNER JOIN dbo.MatchDayParticipants MDPC ON M.CreatedByParticipantId = MDPC.Id
INNER JOIN dbo.Accounts MDPCA ON MDPC.AccountId = MDPCA.Id
INNER JOIN dbo.MatchDayParticipants MDP1 ON M.Participant1Id = MDP1.Id
INNER JOIN dbo.Accounts MDP1A ON MDP1.AccountId = MDP1A.Id
INNER JOIN dbo.MatchDayParticipants MDP2 ON M.Participant2Id = MDP2.Id
INNER JOIN dbo.Accounts MDP2A ON MDP2.AccountId = MDP2A.Id
WHERE M.Id IN (@MatchIds);";

            using (var multi = await this.Connection.QueryMultipleAsync(this.AsCommand(sql, new {MatchIds = matchIds}, token)))
            {
                var matches = (await multi.ReadAsync<Match>()).ToList();
                var participants = (await multi.ReadAsync()).ToList();

                foreach (var match in matches)
                {
                    var participantsForMatch = participants.First(f => f.MatchId == match.Id);
                    match.CreatedBy = new MatchDayParticipant
                    {
                        Id = participantsForMatch.CreatedParticipantId,
                        Account = new Account
                        {
                            Id = participantsForMatch.CreatedAccountId,
                            EmailAddress = participantsForMatch.CreatedEmailAddress,
                        }
                    };
                    match.Participant1 = new MatchDayParticipant
                    {
                        Id = participantsForMatch.Participant1Id,
                        Account = new Account
                        {
                            Id = participantsForMatch.Participant1AccountId,
                            EmailAddress = participantsForMatch.Participant1EmailAddress,
                        }
                    };
                    match.Participant2 = new MatchDayParticipant
                    {
                        Id = participantsForMatch.Participant2Id,
                        Account = new Account
                        {
                            Id = participantsForMatch.Participant2AccountId,
                            EmailAddress = participantsForMatch.Participant2EmailAddress,
                        }
                    };
                }

                return matches;
            }
        }
        private Task<int> InsertMatchAsync(Match match, CancellationToken token)
        {
            Guard.NotNull(match, nameof(match));
            Guard.NotNull(match.CreatedBy, nameof(match.CreatedBy));
            Guard.NotNull(match.Participant1, nameof(match.Participant1));
            Guard.NotNull(match.Participant2, nameof(match.Participant2));

            const string sql = @"
INSERT INTO dbo.Matches (MatchDayId, Number, CreatedByParticipantId, Participant1Id, Participant2Id, StartTime)
VALUES 
(
	@MatchDayId, 
	(SELECT COUNT(*) + 1 FROM dbo.Matches M WHERE M.MatchDayId = @MatchDayId),
	(SELECT MDP.Id FROM dbo.MatchDayParticipants MDP WHERE MDP.MatchDayId = @MatchDayId AND MDP.Id = @CreatedByParticipantId),
	(SELECT MDP.Id FROM dbo.MatchDayParticipants MDP WHERE MDP.MatchDayId = @MatchDayId AND MDP.Id = @Participant1Id),
	(SELECT MDP.Id FROM dbo.MatchDayParticipants MDP WHERE MDP.MatchDayId = @MatchDayId AND MDP.Id = @Participant2Id),
	@StartTime
);

SELECT SCOPE_IDENTITY();";

            var parameters = new
            {
                MatchDayId = match.MatchDayId,
                CreatedByParticipantId = match.CreatedBy.Id,
                Participant1Id = match.Participant1.Id,
                Participant2Id = match.Participant2.Id,
                StartTime = match.StartTime
            };
            return this.Connection.ExecuteScalarAsync<int>(this.AsCommand(sql, parameters, token));
        }
        private Task UpdateMatchAsyncInternal(Match match, CancellationToken token)
        {
            Guard.NotNull(match, nameof(match));
            Guard.NotZeroOrNegative(match.Id, nameof(match.Id));
            Guard.NotZeroOrNegative(match.Participant1Points, nameof(match.Participant1Points));
            Guard.NotZeroOrNegative(match.Participant2Points, nameof(match.Participant2Points));

            const string sql = @"
UPDATE dbo.Matches
SET 
	Participant1Points = @Participant1Points,
	Participant2Points = @Participant2Points,
	StartTime = @StartTime,
	EndTime = @EndTime
WHERE Id = @MatchId";
            var parameters = new
            {
                MatchId = match.Id,
                Participant1Points = match.Participant1Points,
                Participant2Points = match.Participant2Points,
                StartTime = match.StartTime,
                EndTime = match.EndTime
            };

            return this.Connection.ExecuteAsync(this.AsCommand(sql, parameters, token));
        }
        #endregion
    }
}