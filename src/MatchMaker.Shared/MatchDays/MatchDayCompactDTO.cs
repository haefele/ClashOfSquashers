using System;
using System.Collections.Generic;
using MatchMaker.Shared.Accounts;

namespace MatchMaker.Shared.MatchDays
{
    public class MatchDayCompactDTO
    {
        public int Id { get; set; }
        public DateTime When { get; set; }
        public List<AccountCompactDTO> Participants { get; set; }
        public int MatchCount { get; set; }
    }
}