using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Databases.Repositories.Matches
{
    public interface IMatchRepository
    {
        Task<Match> GetMatchAsync(int matchId, CancellationToken token);
        Task<List<Match>> GetMatchesAsync(int matchDayId, CancellationToken token);
        Task<Match> CreateMatchAsync(Match match, CancellationToken token);
        Task UpdateMatchAsync(Match match, CancellationToken token);
    }
}