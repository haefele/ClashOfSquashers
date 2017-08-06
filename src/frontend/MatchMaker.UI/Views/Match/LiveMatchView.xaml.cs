using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.UI.Services.MatchDays;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatchMaker.UI.Views.Match
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LiveMatchView : ContentPage
	{
        public LiveMatchViewModel ViewModel => this.BindingContext as LiveMatchViewModel;
        
	    public LiveMatchView(int matchDayId)
	    {
            this.InitializeComponent();
	        this.BindingContext = new LiveMatchViewModel(matchDayId, DependencyService.Get<IMatchDaysService>());
	        this.Appearing += this.OnAppearing;
	    }

	    private async void OnAppearing(object sender, EventArgs eventArgs)
	    {
	        await this.ViewModel.OnActivate();
	    }
    }
}