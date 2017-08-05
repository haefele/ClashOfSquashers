using MatchMaker.UI.Views.Shell;
using Xamarin.Forms;

[assembly: Dependency(typeof(MatchMaker.UI.Services.Navigation.NavigationService))]
namespace MatchMaker.UI.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public void NavigateToShell()
        {
            Application.Current.MainPage = new ShellView();
        }
    }
}