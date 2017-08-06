using System.Collections.ObjectModel;
using System.Linq;
using MatchMaker.UI.Helpers;
using MatchMaker.UI.Services.Navigation;
using MatchMaker.UI.Services.ShellNavigation;
using MatchMaker.UI.Views.Main;

namespace MatchMaker.UI.Views.Shell
{
    public class ShellViewMasterViewModel : ObservableObject
    {
        private readonly IShellNavigationService _shellNavigationService;

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
                    this._shellNavigationService.SetShellDetailPage(value);
            }
        }

        public ShellViewMasterViewModel(IShellNavigationService shellNavigationService)
        {
            this._shellNavigationService = shellNavigationService;

            // default pages
            this.MenuItems = new ObservableCollection<ShellViewMenuItem>
            {
                new ShellViewMenuItem {Page = new MainView(), TargetType = typeof(MainView), Title = "Overview"}
            };

            this.SelectedMenuItem = this.MenuItems.First<ShellViewMenuItem>();
        }
    }
}