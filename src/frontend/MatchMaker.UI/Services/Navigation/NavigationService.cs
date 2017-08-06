using System.Threading.Tasks;
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

        public void NavigateToNewMatch()
        {
            this._shell.ActivateItem(new ShellViewMenuItem
            {
                TargetType = typeof(MatchDayView),
                Title = "Matchday"
            });
        }

        public async Task ShowModalWindow(ContentPage page)
        {
            var temp = Application.Current.MainPage.Navigation;
            await this._shell.Navigation.PushModalAsync(page);
        }
    }
}