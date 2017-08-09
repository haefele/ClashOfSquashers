using MatchMaker.Api.Databases.Repositories.Accounts;

namespace MatchMaker.Api.Databases
{
    public interface IDatabaseSession
    {
        IMatchDayRepository MatchDayRepository { get; }
        IAccountRepository AccountRepository { get; }

        void Commit();
    }
}