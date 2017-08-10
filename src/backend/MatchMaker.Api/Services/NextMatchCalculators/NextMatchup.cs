using MatchMaker.Shared.Common;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Services.NextMatchCalculators
{
    public class NextMatchup
    {
        public NextMatchup(MatchDayParticipant participant1, MatchDayParticipant participant2)
        {
            Guard.NotNull(participant1, nameof(participant1));
            Guard.NotNull(participant2, nameof(participant2));

            this.Participant1 = participant1;
            this.Participant2 = participant2;
        }

        public MatchDayParticipant Participant1 { get; }
        public MatchDayParticipant Participant2 { get; }
    }
}