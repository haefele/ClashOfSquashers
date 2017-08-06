using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatchMaker.Shared.MatchDays;
using MatchMaker.UI.Common;
using MatchMaker.UI.Services.MatchDays;
using MatchMaker.UI.Services.Navigation;
using Xamarin.Forms;

namespace MatchMaker.UI.Views.MatchDay.MatchDayConfigurator
{
    public class MatchDayConfiguratorViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IMatchDaysService _matchDaysService;
        
        public AsyncCommand CreateCommand { get; }

        public Command CancelCommand { get; }

        public MatchDayConfiguratorViewModel(INavigationService navigationService, IMatchDaysService matchDaysService)
        {
            this._navigationService = navigationService;
            this._matchDaysService = matchDaysService;

            this.CreateCommand = new AsyncCommand(_ => this.Create());
            this.CancelCommand = new Command(_ => this.MatchCreationCanceled?.Invoke(this, EventArgs.Empty));
        }

        private async Task Create()
        {
            var response = await this._matchDaysService.CreateNewMatchDay(new List<int> { 1, 2 }, DateTime.Now);
            
            this.MatchCreated?.Invoke(this, new MatchDayCreatedEventArgs(response));
        }

        public event EventHandler<MatchDayCreatedEventArgs> MatchCreated;

        public EventHandler MatchCreationCanceled;
        
        public class MatchDayCreatedEventArgs : EventArgs
        {
            public MatchDayCompact MatchDay { get; }

            public MatchDayCreatedEventArgs(MatchDayCompact matchDay)
            {
                this.MatchDay = matchDay;
            }
        }
    }
}