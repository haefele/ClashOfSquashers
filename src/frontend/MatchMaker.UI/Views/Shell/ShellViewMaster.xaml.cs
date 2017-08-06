using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.UI.Services.Navigation;
using MatchMaker.UI.Services.ShellNavigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatchMaker.UI.Views.Shell
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShellViewMaster : ContentPage
    {
        public ShellViewMasterViewModel ViewModel => this.BindingContext as ShellViewMasterViewModel;

        public ShellViewMaster()
        {
            this.InitializeComponent();

            this.BindingContext = new ShellViewMasterViewModel(DependencyService.Get<IShellNavigationService>());
        }
    }
}