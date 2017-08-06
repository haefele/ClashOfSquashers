using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatchMaker.UI.Views.Shell
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShellView : MasterDetailPage
    {
        public ShellView()
        {
            this.InitializeComponent();
            this.MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as ShellViewMenuItem;
            if (item == null)
                return;

           this.ActivateItem(item, false);
        }

        public void ActivateItem(ShellViewMenuItem item, bool overrideExisting)
        {
            if (overrideExisting && this.MasterPage.ViewModel.MenuItems.Any(f => f.TargetType == item.TargetType))
            {
                this.MasterPage.ViewModel.MenuItems.Remove(this.MasterPage.ViewModel.MenuItems.First(f => f.TargetType == item.TargetType));
                this.MasterPage.ViewModel.MenuItems.Add(item);
            }
            else if(this.MasterPage.ViewModel.MenuItems.Contains(item) == false)
            {
                this.MasterPage.ViewModel.MenuItems.Add(item);
            }

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            this.Detail = new NavigationPage(page);
            this.IsPresented = false;

            this.MasterPage.ListView.SelectedItem = null;
        }
    }
}