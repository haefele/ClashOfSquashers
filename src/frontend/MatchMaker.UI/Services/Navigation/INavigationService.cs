using System.Threading.Tasks;
using MatchMaker.UI.Views.Shell;
using Xamarin.Forms;

namespace MatchMaker.UI.Services.Navigation
{
    public interface INavigationService
    {
        void NavigateToShell();
        void NavigateToNewMatchDay();

        void SetShellDetailPage(ShellViewMenuItem item);
        void ActivateShellItem(ShellViewMenuItem item);
        void ReactivateShellItem(ShellViewMenuItem item, bool overrideExisting);

        Task ShowModalWindow(ContentPage page);
        Task PopModalWindow(bool animated = false);
        Task PopTopPage(bool animated = false);
    }
}