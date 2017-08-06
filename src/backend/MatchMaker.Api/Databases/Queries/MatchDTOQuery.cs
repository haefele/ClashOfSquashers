using System.Threading.Tasks;
using MatchMaker.Api.Entities;
using MatchMaker.Shared.Accounts;
using MatchMaker.Shared.MatchDays;
using NPoco;

namespace MatchMaker.Api.Databases.Queries
{
    public class MatchDTOQuery : IQuery<MatchDTO>
    {
        private readonly int _matchId;

        public static MatchDTOQuery For(int matchId)
        {
            return new MatchDTOQuery(matchId);
        }

        private MatchDTOQuery(int matchId)
        {
            this._matchId = matchId;
        }

        public async Task<MatchDTO> ExecuteAsync(IDatabase database)
        {
            var match = await database.SingleOrDefaultByIdAsync<Match>(this._matchId);
            
            return new MatchDTO
            {
                Id = match.Id,
                MatchDayId = match.MatchDayId,
                StartTime = match.StartTime,
                EndTime = match.EndTime,
                Number = match.Number,
                Participant1Points = match.Participant1Points,
                Participant2Points = match.Participant2Points,
                CreatedBy = await database.QueryAsync(AccountCompactDTOQuery.For(match.CreatedByAccountId)),
                Participant1 = await database.QueryAsync(AccountCompactDTOQuery.For(match.Participant1AccountId)),
                Participant2 = await database.QueryAsync(AccountCompactDTOQuery.For(match.Participant2AccountId)),
            };
        }
    }
}