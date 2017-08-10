using System;
using MatchMaker.Shared.Accounts;

namespace MatchMaker.Shared.MatchDays
{
    public class Match
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int MatchDayId { get; set; }
        public MatchDayParticipant CreatedBy { get; set; }
        public MatchDayParticipant Participant1 { get; set; }
        public MatchDayParticipant Participant2 { get; set; }
        public int Participant1Points { get; set; }
        public int Participant2Points { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}