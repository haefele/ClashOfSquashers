using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Api.Databases;
using MatchMaker.Api.Services.NextMatchCalculators;
using MatchMaker.Shared.Common;
using MatchMaker.Shared.MatchDays;
using Microsoft.AspNetCore.Mvc;
using NPoco;

namespace MatchMaker.Api.Controllers
{
    [Route("MatchDays")]
    public class MatchesController : Controller
    {
        private readonly IDatabaseSession _databaseSession;
        private readonly INextMatchCalculator _nextMatchCalculator;

        public MatchesController(IDatabaseSession databaseSession, INextMatchCalculator nextMatchCalculator)
        {
            Guard.NotNull(databaseSession, nameof(databaseSession));
            Guard.NotNull(nextMatchCalculator, nameof(nextMatchCalculator));

            this._databaseSession = databaseSession;
            this._nextMatchCalculator = nextMatchCalculator;
        }

        [HttpGet]
        [Route("{matchDayId:int}/Matches")]
        public async Task<IActionResult> GetMatches(int matchDayId, CancellationToken token)
        {
            if (matchDayId <= 0)
                return this.BadRequest();

            var matchDay = await this._databaseSession.MatchDayRepository.GetMatchDayAsync(matchDayId, token);

            if (matchDay == null)
                return this.NotFound();

            var matches = await this._databaseSession.MatchRepository.GetMatchesAsync(matchDayId, token);
            return this.Ok(matches);
        }

        [HttpGet]
        [Route("{matchDayId:int}/Matches/{matchId:int}")]
        public async Task<IActionResult> GetMatch(int matchDayId, int matchId, CancellationToken token)
        {
            if (matchDayId <= 0 || matchId <= 0)
                return this.BadRequest();

            var match = await this._databaseSession.MatchRepository.GetMatchAsync(matchId, token);

            if (match == null || match.MatchDayId != matchDayId)
                return this.NotFound();

            return this.Ok(match);
        }

        [HttpGet]
        [Route("{matchDayId:int}/Matches/Next")]
        public async Task<IActionResult> GetNextMatch(int matchDayId, CancellationToken token)
        {
            if (matchDayId <= 0)
                return this.BadRequest();

            var matchDay = await this._databaseSession.MatchDayRepository.GetMatchDayAsync(matchDayId, token);

            if (matchDay == null)
                return this.NotFound();

            var matches = await this._databaseSession.MatchRepository.GetMatchesAsync(matchDay.Id, token);
            var nextMatchup = this._nextMatchCalculator.CalculateNextMatch(matchDay, matches);

            var result = new Match
            {
                Id = 0,
                Participant1 = nextMatchup.Participant1,
                Participant2 = nextMatchup.Participant2,
                CreatedBy = null,
                MatchDayId = matchDay.Id,
                Number = matches.Any() 
                    ? matches.Max(f => f.Number) + 1
                    : 1,
                Participant1Points = 0,
                Participant2Points = 0,
                StartTime = null,
                EndTime = null
            };

            return this.Ok(result);
        }

        [HttpPost]
        [Route("{matchDayId:int}/Matches")]
        public async Task<IActionResult> SaveMatch(int matchDayId, [FromBody] Match match, CancellationToken token)
        {
            if (matchDayId <= 0 || match == null || match.Participant1 == null || match.Participant2 == null)
                return this.BadRequest();

            var matchDay = await this._databaseSession.MatchDayRepository.GetMatchDayAsync(matchDayId, token);

            if (matchDay.Participants.Any(f => f.Id == match.Participant1.Id) == false)
                return this.BadRequest();

            if (matchDay.Participants.Any(f => f.Id == match.Participant2.Id) == false)
                return this.BadRequest();
            
            match.MatchDayId = matchDayId;

            Match result = await this._databaseSession.MatchRepository.CreateMatchAsync(match, token);
            return this.Created(string.Empty, result);
        }

        [HttpPatch]
        [Route("{matchDayId:int}/Matches/{matchId:int}")]
        public async Task<IActionResult> UpdateMatch(int matchDayId, int matchId, [FromBody] Match match, CancellationToken token)
        {
            if (matchDayId <= 0 || matchId <= 0 || match == null)
                return this.BadRequest();

            var existingMatch = await this._databaseSession.MatchRepository.GetMatchAsync(matchId, token);

            if (existingMatch == null || existingMatch.MatchDayId != matchDayId)
                return this.NotFound();

            existingMatch.StartTime = match.StartTime;
            existingMatch.EndTime = match.EndTime;
            existingMatch.Participant1Points = match.Participant1Points;
            existingMatch.Participant2Points = match.Participant2Points;

            await this._databaseSession.MatchRepository.UpdateMatchAsync(existingMatch, token);

            return this.Ok(existingMatch);
        }
    }
}