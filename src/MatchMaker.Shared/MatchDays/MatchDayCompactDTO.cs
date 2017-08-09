using System;
using System.Collections.Generic;
using MatchMaker.Shared.Accounts;

namespace MatchMaker.Shared.MatchDays
{
    public class MatchDay
    {
        public int Id { get; set; }
        public DateTime When { get; set; }
        public List<Account> Participants { get; set; }
        public int MatchCount { get; set; }
    }
}