using System;
using System.Collections.Generic;

namespace MatchMaker.Shared.MatchDays
{
    public class CreateMatchDayData
    {
        public DateTime When { get; set; }
        public List<int> ParticipantIds { get; set; }
    }
}