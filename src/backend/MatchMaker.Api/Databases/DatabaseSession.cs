using System.Data;
using MatchMaker.Api.Databases.Repositories.Accounts;
using MatchMaker.Shared.Common;

namespace MatchMaker.Api.Databases
{
    public class DatabaseSession : IDatabaseSession
    {
        public IDbTransaction Transaction { get; }
        public IMatchDayRepository MatchDayRepository { get; }
        public IAccountRepository AccountRepository { get; }

        public DatabaseSession(IDbTransaction transaction, IMatchDayRepository matchDayRepository, IAccountRepository accountRepository)
        {
            Guard.NotNull(transaction, nameof(transaction));
            Guard.NotNull(matchDayRepository, nameof(matchDayRepository));
            Guard.NotNull(accountRepository, nameof(accountRepository));

            this.Transaction = transaction;
            this.AccountRepository = accountRepository;
            this.MatchDayRepository = matchDayRepository;
        }

        public void Commit()
        {
            this.Transaction.Commit();
        }
    }
}