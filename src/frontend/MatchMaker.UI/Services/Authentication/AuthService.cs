using MatchMaker.Shared.Common;
using MatchMaker.UI.Services.ApiClient;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(MatchMaker.UI.Services.Authentication.AuthService))]
namespace MatchMaker.UI.Services.Authentication
{
    public class AuthService : IAuthService
    {
        public IApiClientService ApiClient;

        public AuthService()
        {
            this.ApiClient = DependencyService.Get<IApiClientService>();;
        }

        public async Task Register(string email, string password)
        {
            Guard.NotNullOrWhiteSpace(email, nameof(email));
            Guard.NotNullOrWhiteSpace(password, nameof(password));

            await this.ApiClient.Register(email, password);
        }

        public async Task Login(string email, string password)
        {
            Guard.NotNullOrWhiteSpace(email, nameof(email));
            Guard.NotNullOrWhiteSpace(password, nameof(password));

            await this.ApiClient.Login(email, password);
        }
    }
}