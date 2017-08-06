using System;

namespace MatchMaker.Api.Entities
{
    public class Match
    {
        public int Id { get; set; }
        public int MatchDayId { get; set; }
        public int Number { get; set; }
        public int CreatedByAccountId { get; set; }
        public int Participant1AccountId { get; set; }
        public int Participant2AccountId { get; set; }
        public int Participant1Points { get; set; }
        public int Participant2Points { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}