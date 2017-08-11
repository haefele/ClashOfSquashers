using System.Data;
using System.Threading;
using Dapper;
using MatchMaker.Shared.Common;

namespace MatchMaker.Api.Databases.Repositories
{
    public abstract class BaseRepository
    {
        public IDbTransaction Transaction { get; }
        public IDbConnection Connection { get; }

        protected BaseRepository(IDbTransaction transaction)
        {
            Guard.NotNull(transaction, nameof(transaction));

            this.Connection = transaction.Connection;
            this.Transaction = transaction;
        }

        protected CommandDefinition AsCommand(string sql, object parameters, CancellationToken token)
        {
            return new CommandDefinition(sql, parameters, this.Transaction, cancellationToken: token);
        }
    }
}