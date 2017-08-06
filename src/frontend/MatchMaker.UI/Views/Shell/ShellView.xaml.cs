using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.UI.Services.ShellNavigation;
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

            DependencyService.Get<IShellNavigationService>().RegisterShell(this);
        }
    }
}