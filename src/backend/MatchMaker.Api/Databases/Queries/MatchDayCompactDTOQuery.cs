using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchMaker.Api.Entities;
using MatchMaker.Shared.Accounts;
using MatchMaker.Shared.MatchDays;
using NPoco;

namespace MatchMaker.Api.Databases.Queries
{
    public class MatchDayCompactDTOQuery : IQuery<MatchDayCompactDTO>
    {
        private readonly int _matchDayId;

        public static MatchDayCompactDTOQuery For(int matchDayId)
        {
            return new MatchDayCompactDTOQuery(matchDayId);
        }

        private MatchDayCompactDTOQuery(int matchDayId)
        {
            this._matchDayId = matchDayId;
        }

        public async Task<MatchDayCompactDTO> ExecuteAsync(IDatabase database)
        {
            const string sql = "SELECT * " +
                               "FROM MatchDays M " +
                               "WHERE M.Id = @0;" +

                               "SELECT A.Id, A.EmailAddress " +
                               "FROM MatchDayParticipants MDP " +
                               "INNER JOIN Accounts A ON A.Id = MDP.AccountId " +
                               "WHERE MDP.MatchDayId = @0;" +

                               "SELECT COUNT(*) " +
                               "FROM dbo.Matches M " +
                               "WHERE M.MatchDayId = @0";

            var result = database.FetchMultiple<MatchDay, Dictionary<string, object>, int>(sql, this._matchDayId);

            var matchDay = result.Item1.FirstOrDefault();
            var participants = result.Item2;
            var matchCount = result.Item3.FirstOrDefault();

            if (matchDay == null)
                return null;

            return new MatchDayCompactDTO
            {
                Id = matchDay.Id,
                When = matchDay.When,
                MatchCount = matchCount,
                Participants = participants
                    .Select(f => new AccountCompactDTO
                    {
                        Id = (int) f["Id"],
                        EmailAddress = (string) f["EmailAddress"]
                    })
                    .ToList()
            };
        }
    }
}