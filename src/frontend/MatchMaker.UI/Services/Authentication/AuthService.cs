using MatchMaker.Shared.Common;
using MatchMaker.UI.Services.ApiClient;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(MatchMaker.UI.Services.Authentication.AuthService))]
namespace MatchMaker.UI.Services.Authentication
{
    public class AuthService : IAuthService
    {
        public IApiClientService ApiClient => DependencyService.Get<IApiClientService>();

        private string _token;

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

            this._token = await this.ApiClient.Login(email, password);
        }
    }
}