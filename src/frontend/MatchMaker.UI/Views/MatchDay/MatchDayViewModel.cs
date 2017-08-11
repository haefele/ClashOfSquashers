using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MatchMaker.Shared.MatchDays;
using MatchMaker.UI.Common;
using MatchMaker.UI.Services.MatchDays;
using MatchMaker.UI.Services.Navigation;
using MatchMaker.UI.Views.Match;
using MatchMaker.UI.Views.MatchDay.MatchDayConfigurator;
using MatchMaker.UI.Views.Shell;
using Xamarin.Forms;

namespace MatchMaker.UI.Views.MatchDay
{
    public class MatchDayViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IMatchDaysService _matchDaysService;

        private bool _isNew;

        private Shared.MatchDays.MatchDay _matchDay;
        private ObservableCollection<Shared.MatchDays.Match> _matches;

        public bool IsNew
        {
            get { return this._isNew; }
            set { this.SetProperty(ref this._isNew, value); }
        }

        public Shared.MatchDays.MatchDay MatchDay
        {
            get { return this._matchDay; }
            set
            {
                this.SetProperty(ref this._matchDay, value); 
                this.NextMatchCommand.OnCanExecuteChagend();
            }
        }

        public ObservableCollection<Shared.MatchDays.Match> Matches
        {
            get { return this._matches; }
            set { this.SetProperty(ref this._matches, value); }
        }

        public AsyncCommand NextMatchCommand { get; }

        public MatchDayViewModel(INavigationService navigationService, IMatchDaysService matchDaysService)
        {
            this._navigationService = navigationService;
            this._matchDaysService = matchDaysService;

            this.NextMatchCommand = new AsyncCommand(_ => this.NextMatch(), CanNextMatch);

            this.Matches = new ObservableCollection<Shared.MatchDays.Match>();

            this.Title = "Current Matchday";
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
            var newMatchView = new LiveMatchView(this.MatchDay.Id);

            newMatchView.ViewModel.MatchCreated += async (sender, args) =>
            {
                this.Matches.Add(args.Match);

                await this._navigationService.PopModalWindow();
            };
            newMatchView.ViewModel.MatchCreationCanceled += async (sender, args) =>
            {
                await this._navigationService.PopModalWindow();
            };

            await this._navigationService.ShowModalWindow(newMatchView);
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
                    this._navigationService.NavigateToShell();
                };

                await this._navigationService.ShowModalWindow(configView);
                this.IsNew = false;
            }
            else
            {
                
            }
        }

        public Type Type => null;
    }
}