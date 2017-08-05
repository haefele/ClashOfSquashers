using System.Threading.Tasks;
using MatchMaker.Shared.Accounts;
using MatchMaker.UI.Services.ApiClient;
using Xamarin.Forms;

[assembly: Dependency(typeof(MatchMaker.UI.Services.Authentication.AuthService))]
namespace MatchMaker.UI.Services.Authentication
{
    public class AuthService : IAuthService
    {
        public IApiClientService ApiClient => DependencyService.Get<IApiClientService>();

        public async Task Register(string email, string password)
        {
            await this.ApiClient.Register(email, password);
        }

        public async Task Login(string email, string password)
        {
            await this.ApiClient.Login(email, password);
        }
    }
}