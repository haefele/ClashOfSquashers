using System.Data;
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


        public Task InsertAsync(MatchDay matchDay)
        {
            throw new System.NotImplementedException();
        }
    }
}