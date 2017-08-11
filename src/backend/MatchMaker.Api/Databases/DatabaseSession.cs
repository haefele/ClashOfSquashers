using System.Data;
using MatchMaker.Api.Databases.Repositories.Accounts;
using MatchMaker.Api.Databases.Repositories.MatchDays;
using MatchMaker.Api.Databases.Repositories.Matches;
using MatchMaker.Shared.Common;

namespace MatchMaker.Api.Databases
{
    public class DatabaseSession : IDatabaseSession
    {
        public IDbTransaction Transaction { get; }
        public IMatchDayRepository MatchDayRepository { get; }
        public IMatchRepository MatchRepository { get; }
        public IAccountRepository AccountRepository { get; }

        public DatabaseSession(IDbTransaction transaction, IMatchDayRepository matchDayRepository, IMatchRepository matchRepository, IAccountRepository accountRepository)
        {
            Guard.NotNull(transaction, nameof(transaction));
            Guard.NotNull(matchDayRepository, nameof(matchDayRepository));
            Guard.NotNull(matchRepository, nameof(matchRepository));
            Guard.NotNull(accountRepository, nameof(accountRepository));

            this.Transaction = transaction;
            this.MatchDayRepository = matchDayRepository;
            this.MatchRepository = matchRepository;
            this.AccountRepository = accountRepository;
        }

        public void Commit()
        {
            this.Transaction.Commit();
        }
    }
}