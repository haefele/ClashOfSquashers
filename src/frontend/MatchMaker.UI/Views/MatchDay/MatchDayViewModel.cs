using System;
using System.Threading.Tasks;
using MatchMaker.Shared.MatchDays;
using MatchMaker.UI.Common;
using MatchMaker.UI.Services.MatchDays;
using MatchMaker.UI.Services.Navigation;
using MatchMaker.UI.Views.MatchDay.MatchDayConfigurator;
using Xamarin.Forms;

namespace MatchMaker.UI.Views.MatchDay
{
    public class MatchDayViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IMatchDaysService _matchDaysService;

        private bool _isNew;
        private MatchDayCompact _matchDay;

        public bool IsNew
        {
            get { return this._isNew; }
            set { this.SetProperty(ref this._isNew, value); }
        }

        public MatchDayCompact MatchDay
        {
            get { return this._matchDay; }
            set
            {
                this.SetProperty(ref this._matchDay, value); 
                this.NextMatchCommand.OnCanExecuteChagend();
            }
        }

        public AsyncCommand NextMatchCommand { get; }

        public MatchDayViewModel(INavigationService navigationService, IMatchDaysService matchDaysService)
        {
            this._navigationService = navigationService;
            this._matchDaysService = matchDaysService;

            this.NextMatchCommand = new AsyncCommand(_ => this.NextMatch(), CanNextMatch);

            this.IsNew = true;
        }

        private bool CanNextMatch(object o)
        {
            if (this.MatchDay == null)
                return false;

            return true;
        }

        private async Task NextMatch()
        {
            this._navigationService.NavigateToNewMatch(this.MatchDay.Id);
        }

        public async Task OnActivate()
        {
            if (this.IsNew)
            {
                var configView = new MatchDayConfiguratorView();

                configView.ViewModel.MatchCreated += async (sender, args) =>
                {
                    this.MatchDay = args.MatchDay;

                    await this._navigationService.PopModalWindow();
                };
                configView.ViewModel.MatchCreationCanceled += async (sender, args) =>
                {
                    await this._navigationService.PopModalWindow();
                };

                await this._navigationService.ShowModalWindow(configView);
                this.IsNew = false;
            }
            else
            {
                
            }
        }
    }
}