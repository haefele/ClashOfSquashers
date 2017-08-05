using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.UI.Services.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatchMaker.UI.Views.MatchDay
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MatchDayView : ContentPage
	{
        public MatchDayViewModel ViewModel => this.BindingContext as MatchDayViewModel;

		public MatchDayView ()
		{
			this.InitializeComponent();

            this.BindingContext = new MatchDayViewModel(DependencyService.Get<INavigationService>());
		    this.Appearing += this.OnAppearing;
        }

	    private void OnAppearing(object sender, EventArgs eventArgs)
	    {
	        this.ViewModel.OnActivate();
	    }
    }
}