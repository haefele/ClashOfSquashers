using System.Threading.Tasks;

namespace MatchMaker.UI.Services.ApiClient
{
    public interface IApiClientService
    {
        Task Register(string userName, string password);
    }
}