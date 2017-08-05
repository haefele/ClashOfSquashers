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
        public List<AccountCompact> Participants { get; set; }
        public int MatchCount { get; set; }
    }
    
    public class Match
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public AccountCompact CreatedBy { get; set; }
        public AccountCompact Participant1 { get; set; }
        public AccountCompact Participant2 { get; set; }
        public int Participant1Points { get; set; }
        public int Participant2Points { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    public class AccountCompact
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
    }
}