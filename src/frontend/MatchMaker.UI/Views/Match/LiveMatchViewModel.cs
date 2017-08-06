using System.Threading.Tasks;
using MatchMaker.Shared.MatchDays;
using MatchMaker.UI.Common;
using MatchMaker.UI.Services.MatchDays;

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

        public LiveMatchViewModel(int matchDayId, IMatchDaysService matchDaysService)
        {
            this._matchDaysService = matchDaysService;

            this.AddResultsCommand = new AsyncCommand(_ => this.AddResults());

            this._matchDayId = matchDayId;
        }

        private async Task AddResults()
        {

        }

        public async Task OnActivate()
        {
            this.Match = await this._matchDaysService.GetNextMatch(this._matchDayId);

        }
    }
}