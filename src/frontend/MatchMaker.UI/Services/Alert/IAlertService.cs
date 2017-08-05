using System.Threading.Tasks;

namespace MatchMaker.UI.Services.Alert
{
    public interface IAlertService
    {
        Task DisplayAlert(string title, string message);
    }
}