using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Databases.Repositories.Matches
{
    public interface IMatchRepository
    {
        Task<List<Match>> GetMatchesAsync(int matchDayId, CancellationToken token);
    }
}