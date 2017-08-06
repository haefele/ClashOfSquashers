using System.Collections.ObjectModel;
using System.Linq;
using MatchMaker.UI.Helpers;
using MatchMaker.UI.Services.Navigation;
using MatchMaker.UI.Views.Main;

namespace MatchMaker.UI.Views.Shell
{
    public class ShellViewMasterViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        private ObservableCollection<ShellViewMenuItem> _menuItems;
        private ShellViewMenuItem _selectedMenuItem;

        public ObservableCollection<ShellViewMenuItem> MenuItems
        {
            get { return this._menuItems; }
            set { this.SetProperty(ref this._menuItems, value); }
        }

        public ShellViewMenuItem SelectedMenuItem
        {
            get { return this._selectedMenuItem; }
            set
            {
                this.SetProperty(ref this._selectedMenuItem, value);

                if (value != null)
                    this._navigationService.SetShellDetailPage(value);
            }
        }

        public ShellViewMasterViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

            // default pages
            this.MenuItems = new ObservableCollection<ShellViewMenuItem>
            {
                new ShellViewMenuItem {Page = new MainView(), TargetType = typeof(MainView), Title = "Overview"}
            };

            this.SelectedMenuItem = Enumerable.First<ShellViewMenuItem>(this.MenuItems);
        }
    }
}