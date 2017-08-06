using MatchMaker.Shared.Common;
using MatchMaker.Shared.MatchDays;
using MatchMaker.UI.Services.ApiClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(MatchMaker.UI.Services.MatchDays.MatchDaysService))]
namespace MatchMaker.UI.Services.MatchDays
{
    public class MatchDaysService : IMatchDaysService
    {
        public IApiClientService ApiClient => DependencyService.Get<IApiClientService>();

        public async Task<MatchDayCompact> CreateNewMatchDay(List<int> participantIds, DateTime when)
        {
            Guard.NotNullOrEmpty(participantIds, nameof(participantIds));
            Guard.NotInvalidDateTime(when, nameof(when));

            return await this.ApiClient.CreateNewMatchDay(participantIds, when);
        }

        public async Task<Match> GetNextMatch(int matchDayId)
        {
            Guard.NotLessOrEqual(matchDayId, 0, nameof(matchDayId));

            return await this.ApiClient.GetNextMatch(matchDayId);
        }

        public async Task<Match> SaveMatch(int matchDayId, Match match)
        {
            Guard.NotLessOrEqual(matchDayId, 0, nameof(matchDayId));
            Guard.NotNull(match, nameof(match));

            return await this.ApiClient.SaveMatch(matchDayId, match);
        }
    }
}