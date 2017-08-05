using System;
using System.Collections.Generic;

namespace MatchMaker.Shared.MatchDays
{
    public class CreateMatchDayData
    {
        public DateTime When { get; set; }
        public List<int> ParticipantIds { get; set; }
    }

    public class MatchDayCompact
    {
        public int Id { get; set; }
        public DateTime When { get; set; }
        public List<MatchDayParticipant> Participants { get; set; }
        public int MatchCount { get; set; }
    }

    public class MatchDayParticipant
    {
        public int AccountId { get; set; }
        public string EmailAddress { get; set; }
    }
}