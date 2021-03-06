﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.UI.Services.MatchDays;
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

            this.BindingContext = new MatchDayViewModel(DependencyService.Get<INavigationService>(), DependencyService.Get<IMatchDaysService>());
		    this.Appearing += this.OnAppearing;
        }

	    private async void OnAppearing(object sender, EventArgs eventArgs)
	    {
	        await this.ViewModel.OnActivate();
	    }
    }
}