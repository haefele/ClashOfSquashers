using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Api.Databases;
using MatchMaker.Shared.Common;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Services.NextMatchCalculators
{
    public class NextMatchCalculator : INextMatchCalculator
    {
        public NextMatchup CalculateNextMatch(MatchDay matchDay, List<Match> matches)
        {
            Guard.NotNull(matchDay, nameof(matchDay));

            if (matchDay.Participants.Count < 2)
                throw new Exception("Cannot calculate the next match when there are less than 2 participants.");
            
            var matchupsWithCount = this.GetMatchupsWithCount(matchDay, matches);

            var nextMatchup = matchupsWithCount.First(f => f.count == matchupsWithCount.Select(d => d.count).Min());

            return new NextMatchup(nextMatchup.participant1, nextMatchup.participant2);
        }

        private List<(MatchDayParticipant participant1, MatchDayParticipant participant2, int count)> GetMatchupsWithCount(MatchDay matchDay, List<Match> matches)
        {
            var result = new List<(MatchDayParticipant participant1, MatchDayParticipant participant2, int count)>();

            for (int i = 0; i < matchDay.Participants.Count; i++)
            {
                for (int o = i + 1; o < matchDay.Participants.Count; o++)
                {
                    result.Add((matchDay.Participants[i], matchDay.Participants[o], 0));
                }
            }

            foreach (var match in matches)
            {
                var foundMatch = result.First(f => 
                    f.participant1 == match.Participant1 && f.participant2 == match.Participant2 ||
                    f.participant1 == match.Participant2 && f.participant2 == match.Participant1);

                foundMatch.count++;
            }

            return result;
        }
    }
}