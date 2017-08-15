using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Shared.Accounts;

namespace MatchMaker.Api.Databases.Repositories.Accounts
{
    public interface IAccountRepository
    {
        Task<Account> CreateAsync(string emailAddress, string password, CancellationToken token);
        Task<(Account account, string passwordHash)> GetAccountAndPasswordHashFromEmailAddressAsync(string emailAddress, CancellationToken token);
        Task<Account> GetAccountByIdAsync(int accountId, CancellationToken token);
        Task<List<Account>> SearchAccountsAsync(string searchText, CancellationToken token);
    }
}