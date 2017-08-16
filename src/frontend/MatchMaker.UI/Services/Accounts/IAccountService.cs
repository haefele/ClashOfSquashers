using System.Collections.Generic;
using System.Threading.Tasks;
using MatchMaker.Shared.Accounts;

namespace MatchMaker.UI.Services.Accounts
{
    public interface IAccountService
    {
        Task<IList<Account>> SearchAccounts(string text);
    }
}