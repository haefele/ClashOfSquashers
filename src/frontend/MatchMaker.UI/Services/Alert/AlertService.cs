using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(MatchMaker.UI.Services.Alert.AlertService))]
namespace MatchMaker.UI.Services.Alert
{
    public class AlertService : IAlertService
    {
        public async Task DisplayAlert(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
    }
}