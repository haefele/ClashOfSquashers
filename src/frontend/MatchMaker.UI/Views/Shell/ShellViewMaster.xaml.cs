using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.UI.Views.Main;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatchMaker.UI.Views.Shell
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShellViewMaster : ContentPage
    {
        public ListView ListView;
        public ShellViewMasterViewModel ViewModel => this.BindingContext as ShellViewMasterViewModel;

        public ShellViewMaster()
        {
            this.InitializeComponent();

            this.BindingContext = new ShellViewMasterViewModel();
            this.ListView = this.MenuItemsListView;
        }


    }

    public class ShellViewMasterViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ShellViewMenuItem> MenuItems { get; set; }

        public ShellViewMasterViewModel()
        {
            MenuItems = new ObservableCollection<ShellViewMenuItem>(new[]
            {
                new ShellViewMenuItem { Title = "Overview", TargetType = typeof(MainView)}
            });
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}