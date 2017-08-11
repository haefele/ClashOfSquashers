using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MatchMaker.Shared.Accounts;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Databases.Repositories.Matches
{
    public class MatchRepository : BaseRepository, IMatchRepository
    {
        public MatchRepository(IDbTransaction transaction) 
            : base(transaction)
        {
        }

        public async Task<List<Match>> GetMatchesAsync(int matchDayId, CancellationToken token)
        {
            var matchIds = await this.GetMatchIdsAsync(matchDayId, token);
            return await this.GetMatchesAsync(matchIds, token);
        }

        public Task<Match> CreateMatchAsync(Match match, CancellationToken token)
        {

            throw new System.NotImplementedException();
        }

        #region SQL
        private async Task<List<int>> GetMatchIdsAsync(int matchDayId, CancellationToken token)
        {
            const string sql = @"
SELECT M.Id
FROM dbo.Matches M
WHERE M.MatchDayId = @MatchDayId";

            return (await this.Connection.QueryAsync<int>(this.AsCommand(sql, new {MatchDayId = matchDayId}, token))).ToList();
        }
        private async Task<List<Match>> GetMatchesAsync(List<int> matchIds, CancellationToken token)
        {
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
            const string sql = @"
INSERT INTO dbo.Matches (MatchDayId, Number, CreatedByAccountId, Participant1AccountId, Participant2AccountId, StartTime)
VALUES (@MatchDayId, (SELECT COUNT(*) + 1 FROM dbo.Matches MM WHERE MM.MatchDayId = @MatchDayId), @CreatedByAccountId, @Participant1AccountId, @Participant2AccountId, @StartTime);

SELECT SCOPE_IDENTITY();";

            return this.Connection.ExecuteScalarAsync<int>(this.AsCommand(sql, new { MatchDayId = match.MatchDayId, CreatedByAccountId = match.CreatedBy.Id }, token));
        }
        #endregion
    }
}