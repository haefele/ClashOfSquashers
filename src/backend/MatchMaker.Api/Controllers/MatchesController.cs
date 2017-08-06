using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Api.Database;
using MatchMaker.Shared.MatchDays;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers
{
    [Route("MatchDays")]
    public class MatchesController : Controller
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public MatchesController(IDbConnectionFactory dbConnectionFactory)
        {
            this._dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        [HttpGet]
        [Route("{matchDayId:int}/Matches/Next")]
        public async Task<IActionResult> GetNextMatch(int matchDayId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (matchDayId <= 0)
                return this.BadRequest();

            using (var connection = this._dbConnectionFactory.Create())
            using (var transaction = connection.BeginTransaction())
            {
                var matches = await connection.GetMatchesFromMatchDay(matchDayId, transaction, cancellationToken);
                var participants = await connection.GetParticipantsFromMatchDay(matchDayId, transaction, cancellationToken);

                if (participants.Count == 0)
                    return this.BadRequest();

                var uniqueMatches = this.CreateUniqueMatches(participants);
                foreach (var match in matches)
                {
                    var uniqueMatch = uniqueMatches.FirstOrDefault(f =>
                        f.Participant1 == match.Item1 && f.Participant2 == match.Item2 ||
                        f.Participant1 == match.Item2 && f.Participant1 == match.Item1);

                    uniqueMatch.Count++;
                }

                var nextMatch = uniqueMatches.FirstOrDefault(f => f.Count == uniqueMatches.Select(d => d.Count).Min());
                var accountCompacts = await connection.GetAccountCompacts(new List<int> { nextMatch.Participant1, nextMatch.Participant2 }, transaction, cancellationToken);

                var result = new Match
                {
                    Id = 0,
                    Number = await connection.GetNextMatchNumberForMatchDay(matchDayId, transaction, cancellationToken),
                    MatchDayId = matchDayId,
                    Participant1 = accountCompacts.First(f => f.Id == nextMatch.Participant1),
                    Participant2 = accountCompacts.First(f => f.Id == nextMatch.Participant2),
                    CreatedBy = null,
                    Participant1Points = 0,
                    Participant2Points = 0,
                    StartTime = null,
                    EndTime = null
                };

                return this.Ok(result);
            }
        }
        
        [HttpPost]
        [Route("{matchDayId:int}/Matches")]
        public async Task<IActionResult> SaveMatch(int matchDayId, [FromBody] Match match, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (matchDayId <= 0 || match == null || match.Participant1 == null || match.Participant2 == null)
                return this.BadRequest();

            using (var connection = this._dbConnectionFactory.Create())
            using (var transaction = connection.BeginTransaction())
            {
                List<int> participants = await connection.GetParticipantsFromMatchDay(matchDayId, transaction, cancellationToken);

                if (participants.Contains(match.Participant1.Id) == false)
                    return this.BadRequest();

                if (participants.Contains(match.Participant2.Id) == false)
                    return this.BadRequest();
                
                //match.CreatedBy =  ;
                match.MatchDayId = matchDayId;

                match.Id = await connection.SaveMatch(match, transaction, cancellationToken);

                transaction.Commit();

                return this.Created(string.Empty, match);
            }
        }

        [HttpPut]
        [Route("{matchDayId:int}/Matches/{matchId:int}")]
        public async Task<IActionResult> UpdateMatch(int matchDayId, int matchId, [FromBody] Match match, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var connection = this._dbConnectionFactory.Create())
            using (var transaction = connection.BeginTransaction())
            {
                match.Id = matchId;
                match.MatchDayId = matchDayId;

                await connection.UpdateMatch(match, transaction, cancellationToken);

                transaction.Commit();

                return this.Ok();
            }
        }

        #region Private Methods
        private List<Matchup> CreateUniqueMatches(List<int> participants)
        {
            var result = new List<Matchup>();

            for (int i = 0; i < participants.Count; i++)
            {
                for (int o = i + 1; o < participants.Count; o++)
                {
                    result.Add(new Matchup
                    {
                        Participant1 = participants[i],
                        Participant2 = participants[o]
                    });
                }
            }

            return result;
        }
        #endregion

        #region Internal
        private class Matchup
        {
            public int Participant1 { get; set; }
            public int Participant2 { get; set; }
            public int Count { get; set; }
        }
        #endregion
    }
}