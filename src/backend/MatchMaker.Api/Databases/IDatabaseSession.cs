using MatchMaker.Api.Databases.Repositories.Accounts;
using MatchMaker.Api.Databases.Repositories.MatchDays;
using MatchMaker.Api.Databases.Repositories.Matches;

namespace MatchMaker.Api.Databases
{
    public interface IDatabaseSession
    {
        IMatchDayRepository MatchDayRepository { get; }
        IMatchRepository MatchRepository { get; }
        IAccountRepository AccountRepository { get; }

        void Commit();
    }
}