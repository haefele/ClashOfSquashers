using System.Threading.Tasks;
using MatchMaker.UI.Views.Match;
using MatchMaker.UI.Views.MatchDay;
using MatchMaker.UI.Views.Shell;
using Xamarin.Forms;

[assembly: Dependency(typeof(MatchMaker.UI.Services.Navigation.NavigationService))]
namespace MatchMaker.UI.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private ShellView _shell;

        public void NavigateToShell()
        {
            Application.Current.MainPage = this._shell = new ShellView();
        }

        public void NavigateToNewMatchDay()
        {
            this._shell.ActivateItem(new ShellViewMenuItem
            {
                TargetType = typeof(MatchDayView),
                Title = "Current Matchday"
            }, true);
        }

        public void NavigateToNewMatch(int matchDayId)
        {
            Application.Current.MainPage = new LiveMatchView(matchDayId);
        }

        public async Task ShowModalWindow(ContentPage page)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public async Task PopModalWindow(bool animated = false)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync(animated);
        }

        public async Task PopTopPage(bool animated = false)
        {
            await Application.Current.MainPage.Navigation.PopAsync(animated);
        }
    }
}