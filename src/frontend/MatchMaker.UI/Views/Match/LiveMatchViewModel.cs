using System;
using System.Threading.Tasks;
using MatchMaker.Shared.MatchDays;
using MatchMaker.UI.Common;
using MatchMaker.UI.Services.MatchDays;
using Xamarin.Forms;

namespace MatchMaker.UI.Views.Match
{
    public class LiveMatchViewModel : BaseViewModel
    {
        private readonly int _matchDayId;
        private readonly IMatchDaysService _matchDaysService;
        private Shared.MatchDays.Match _match;

        public Shared.MatchDays.Match Match
        {
            get { return this._match; }
            set { this.SetProperty(ref this._match, value); }
        }

        public AsyncCommand AddResultsCommand { get; }
        public Command CancelCommand { get; }

        public LiveMatchViewModel(int matchDayId, IMatchDaysService matchDaysService)
        {
            this._matchDaysService = matchDaysService;

            this.AddResultsCommand = new AsyncCommand(_ => this.AddResults());
            this.CancelCommand = new Command(_ => this.MatchCreationCanceled?.Invoke(this, EventArgs.Empty));

            this._matchDayId = matchDayId;
        }

        private async Task AddResults()
        {
            var match = await this._matchDaysService.SaveMatch(this._matchDayId, this.Match);
            this.MatchCreated?.Invoke(this, new MatchResultsAddedEventArgs(match));
        }

        public async Task OnActivate()
        {
            this.Match = await this._matchDaysService.GetNextMatch(this._matchDayId);
        }
        public event EventHandler<MatchResultsAddedEventArgs> MatchCreated;

        public EventHandler MatchCreationCanceled;
    }

    public class MatchResultsAddedEventArgs : EventArgs
    {
        public Shared.MatchDays.Match Match { get; }
        
        public MatchResultsAddedEventArgs(Shared.MatchDays.Match match)
        {
            this.Match = match;
        }
    }
}