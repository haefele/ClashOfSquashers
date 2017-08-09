using System.Data;
using System.Threading;
using Dapper;

namespace MatchMaker.Api.Databases.Repositories
{
    public abstract class BaseRepository
    {
        public IDbTransaction Transaction { get; }
        public IDbConnection Connection { get; }

        protected BaseRepository(IDbTransaction transaction)
        {
            this.Connection = transaction.Connection;
            this.Transaction = transaction;
        }

        protected CommandDefinition AsCommand(string sql, object parameters, CancellationToken token)
        {
            return new CommandDefinition(sql, parameters, this.Transaction, cancellationToken: token);
        }
    }
}