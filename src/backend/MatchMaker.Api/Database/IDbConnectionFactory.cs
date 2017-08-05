using System.Data;
using System.Data.Common;

namespace MatchMaker.Api.Database
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
    }
}