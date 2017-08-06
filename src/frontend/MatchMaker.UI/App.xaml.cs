using System;
using System.Threading.Tasks;
using MatchMaker.UI.Views;
using MatchMaker.UI.Views.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MatchMaker.UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SetMainPage();
        }

        public static void SetMainPage()
        {
            Current.MainPage = new LoginView();
        }
    }
}
