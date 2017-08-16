
using System.Collections.Generic;
using System.Threading.Tasks;
using MatchMaker.Shared.Accounts;
using MatchMaker.UI.Services.ApiClient;
using Xamarin.Forms;

[assembly: Dependency(typeof(MatchMaker.UI.Services.Accounts.AccountService))]
namespace MatchMaker.UI.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private IApiClientService _apiClient;

        public AccountService()
        {
            this._apiClient = DependencyService.Get<IApiClientService>();
        }

        public async Task<IList<Account>> SearchAccounts(string text)
        {
            return await this._apiClient.SearchAccount(text);
        }
    }
}