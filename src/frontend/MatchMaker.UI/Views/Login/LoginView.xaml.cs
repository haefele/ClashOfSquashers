using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.UI.Services.Alert;
using MatchMaker.UI.Services.Authentication;
using MatchMaker.UI.Services.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatchMaker.UI.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginViewModel ViewModel => this.BindingContext as LoginViewModel;

        public LoginView()
        {
            this.InitializeComponent();
            
            this.BindingContext = new LoginViewModel(
                DependencyService.Get<IAuthService>(), 
                DependencyService.Get<INavigationService>(),
                DependencyService.Get<IAlertService>());
        }
    }
}