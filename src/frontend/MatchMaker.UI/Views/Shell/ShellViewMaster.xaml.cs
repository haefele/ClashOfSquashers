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

        public ShellViewMaster()
        {
            InitializeComponent();

            BindingContext = new ShellViewMasterViewModel();
            ListView = MenuItemsListView;
        }

        class ShellViewMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<ShellViewMenuItem> MenuItems { get; set; }
            
            public ShellViewMasterViewModel()
            {
                MenuItems = new ObservableCollection<ShellViewMenuItem>(new[]
                {
                    new ShellViewMenuItem { Id = 0, Title = "Page 1", TargetType = typeof(MainView)}
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
}