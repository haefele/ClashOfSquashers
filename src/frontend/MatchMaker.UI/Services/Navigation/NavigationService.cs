using System;
using System.Linq;
using System.Threading.Tasks;
using MatchMaker.UI.Services.ShellNavigation;
using MatchMaker.UI.Views.Match;
using MatchMaker.UI.Views.MatchDay;
using MatchMaker.UI.Views.Shell;
using Xamarin.Forms;

[assembly: Dependency(typeof(MatchMaker.UI.Services.Navigation.NavigationService))]
namespace MatchMaker.UI.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public IShellNavigationService ShellNavigationService;

        public NavigationService()
        {
            this.ShellNavigationService = DependencyService.Get<IShellNavigationService>();
        }

        public void NavigateToShell()
        {
            Application.Current.MainPage = this.ShellNavigationService.Shell ?? new ShellView();
        }

        public void NavigateToNewMatchDay()
        {
            this.ShellNavigationService.ActivateShellItem(new ShellViewMenuItem
            {
                Page = new MatchDayView(),
                TargetType = typeof(MatchDayView),
                Title = "Current Matchday"
            });
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