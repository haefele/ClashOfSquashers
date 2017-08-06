using System.Threading.Tasks;
using Xamarin.Forms;

namespace MatchMaker.UI.Services.Navigation
{
    public interface INavigationService
    {
        void NavigateToShell();
        void NavigateToNewMatchDay();
        void NavigateToNewMatch(int matchDayId);

        Task ShowModalWindow(ContentPage page);
        Task PopModalWindow(bool animated = false);
        Task PopTopPage(bool animated = false);
    }
}