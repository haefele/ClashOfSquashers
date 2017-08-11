using System;
using System.Collections.Generic;

namespace MatchMaker.Shared.MatchDays
{
    public class MatchDay
    {
        public int Id { get; set; }
        public DateTime When { get; set; }
        public List<MatchDayParticipant> Participants { get; set; }
        public int MatchCount { get; set; }
    }
}