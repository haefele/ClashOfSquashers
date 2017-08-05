using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
            this.BindingContext = new LoginViewModel();
        }
    }
}