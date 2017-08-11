using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Databases.Repositories.MatchDays
{
    public interface IMatchDayRepository
    {
        Task<MatchDay> CreateNewAsync(DateTime when, List<int> participantAccountIds, CancellationToken token);
        Task<MatchDay> GetMatchDayAsync(int matchDayId, CancellationToken token);
    }
}