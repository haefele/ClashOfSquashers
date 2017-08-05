using System.Threading.Tasks;

namespace MatchMaker.UI.Services.ApiClient
{
    public interface IApiClientService
    {
        Task Register(string email, string password);

        Task Login(string email, string password);
    }
}