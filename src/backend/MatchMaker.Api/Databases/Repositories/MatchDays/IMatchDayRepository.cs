using System.Threading.Tasks;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Databases
{
    public interface IMatchDayRepository
    {
        Task InsertAsync(MatchDay matchDay);
    }
}