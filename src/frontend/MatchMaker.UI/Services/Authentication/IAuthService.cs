using System.Threading.Tasks;
using MatchMaker.Shared.Accounts;

namespace MatchMaker.UI.Services.Authentication
{
    public interface IAuthService
    {
        Task Register(string email, string password);

        Task Login(string email, string password);
    }
}