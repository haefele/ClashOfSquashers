using System;
using System.Linq;
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
            this.ActivateShellItem(new ShellViewMenuItem
            {
                Page = new MatchDayView(),
                TargetType = typeof(MatchDayView),
                Title = "Current Matchday"
            });
        }

        public void SetShellDetailPage(ShellViewMenuItem item)
        {
            if(this._shell == null)
                return;

            item.Page.Title = item.Title;
            this._shell.Detail = item.Page;
            this._shell.IsPresented = false;
            
        }

        public void ActivateShellItem(ShellViewMenuItem item)
        {
            var masterPage = this._shell?.Master as ShellViewMaster;

            if(masterPage == null)
                return;

            if (masterPage.ViewModel.MenuItems.Contains(item) == false)
            {
                masterPage.ViewModel.MenuItems.Add(item);
            }

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;
        }
        
        public void ReactivateShellItem(ShellViewMenuItem item, bool overrideExisting)
        {

            try
            {
                var masterPage = this._shell?.Master as ShellViewMaster;

                if (masterPage == null)
                    return;

                if (overrideExisting && masterPage.ViewModel.MenuItems.Any(f => f.TargetType == item.TargetType))
                {
                    // replace
                    masterPage.ViewModel.MenuItems.Remove(masterPage.ViewModel.MenuItems.First(f => f.TargetType == item.TargetType));
                    masterPage.ViewModel.MenuItems.Add(item);
                    masterPage.ViewModel.SelectedMenuItem = item;
                }
                else if (overrideExisting == false && masterPage.ViewModel.MenuItems.Any(f => f.TargetType == item.TargetType))
                {
                    // reactivate
                    masterPage.ViewModel.SelectedMenuItem = masterPage.ViewModel.MenuItems.FirstOrDefault(f => f.TargetType == item.TargetType);
                }
            }
            catch (System.Exception e)
            {
                
                throw;
            }
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