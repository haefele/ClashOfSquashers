using System.Threading.Tasks;
using NPoco;

namespace MatchMaker.Api.Databases.Queries
{
    public interface IQuery<T>
    {
        Task<T> ExecuteAsync(IDatabase database);
    }
}