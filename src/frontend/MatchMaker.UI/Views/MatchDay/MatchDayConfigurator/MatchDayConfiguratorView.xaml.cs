using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.UI.Services.MatchDays;
using MatchMaker.UI.Services.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatchMaker.UI.Views.MatchDay.MatchDayConfigurator
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MatchDayConfiguratorView : ContentPage
	{
        public MatchDayConfiguratorViewModel ViewModel => this.BindingContext as MatchDayConfiguratorViewModel;

		public MatchDayConfiguratorView ()
		{
			this.InitializeComponent();
            this.BindingContext = new MatchDayConfiguratorViewModel(
                DependencyService.Get<INavigationService>(), 
                DependencyService.Get<IMatchDaysService>());
		}
    }
}