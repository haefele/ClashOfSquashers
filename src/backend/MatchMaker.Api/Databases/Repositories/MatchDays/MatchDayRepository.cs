using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Databases.Repositories.MatchDays
{
    public class MatchDayRepository : BaseRepository, IMatchDayRepository
    {
        public MatchDayRepository(IDbTransaction transaction) 
            : base(transaction)
        {
        }
        
        public Task<MatchDay> CreateNewAsync(DateTime when, List<int> participantAccountIds, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}