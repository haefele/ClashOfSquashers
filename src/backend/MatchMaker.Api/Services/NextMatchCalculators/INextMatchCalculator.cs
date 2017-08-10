using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Services.NextMatchCalculators
{
    public interface INextMatchCalculator
    {
        NextMatchup CalculateNextMatch(MatchDay matchDay, List<Match> matches);
    }
}