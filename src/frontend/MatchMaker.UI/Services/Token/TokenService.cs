using Xamarin.Forms;

[assembly: Dependency(typeof(MatchMaker.UI.Services.Token.TokenService))]
namespace MatchMaker.UI.Services.Token
{
    public class TokenService : ITokenService
    {

        public TokenService()
        {
            
        }

        public string Token { get; private set; }

        public void SaveToken(string token)
        {
            this.Token = token;
        }
    }
}