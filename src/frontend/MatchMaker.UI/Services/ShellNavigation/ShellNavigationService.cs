using System;
using System.Collections.Generic;
using System.Linq;
using MatchMaker.UI.Views.Shell;
using Xamarin.Forms;

[assembly: Dependency(typeof(MatchMaker.UI.Services.ShellNavigation.ShellNavigationService))]
namespace MatchMaker.UI.Services.ShellNavigation
{
    public class ShellNavigationService : IShellNavigationService
    {
        public ShellView Shell { get; private set; }
        private ShellViewMaster _masterPage => this.Shell.Master as ShellViewMaster;

        public void RegisterShell(ShellView shell)
        {
            if (this.Shell != null)
                throw new System.Exception("Shell already registered");

            this.Shell = shell;
        }

        public void SetShellDetailPage(ShellViewMenuItem item)
        {
            if (this.Shell == null)
                return;

            item.Page.Title = item.Title;
            this.Shell.Detail = new NavigationPage(item.Page);
            this.Shell.IsPresented = false;
        }

        public void ActivateShellItem(ShellViewMenuItem item)
        {
            if (this._masterPage.ViewModel.MenuItems.Contains(item) == false)
            {
                this._masterPage.ViewModel.MenuItems.Add(item);
            }

            // reactivate
            this._masterPage.ViewModel.SelectedMenuItem = this._masterPage.ViewModel.MenuItems.FirstOrDefault(f => f == item);
        }

        public void ReactivateShellItem(ShellViewMenuItem item, bool overrideExisting)
        {
            if (overrideExisting && this._masterPage.ViewModel.MenuItems.Any(f => f.TargetType == item.TargetType))
            {
                // replace
                this._masterPage.ViewModel.MenuItems.Remove(this._masterPage.ViewModel.MenuItems.First(f => f == item));
                this._masterPage.ViewModel.MenuItems.Add(item);
                this._masterPage.ViewModel.SelectedMenuItem = item;
            }
            else if (overrideExisting == false && this._masterPage.ViewModel.MenuItems.Any(f => f.TargetType == item.TargetType))
            {
                // reactivate
                this._masterPage.ViewModel.SelectedMenuItem = item;
            }
        }
    }
}