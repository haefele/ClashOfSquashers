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
            throw new NotImplementedException();

            //if (matchDayId <= 0 || match == null || match.Participant1 == null || match.Participant2 == null)
            //    return this.BadRequest();

            //using (var transaction = this._database.GetTransaction())
            //{
            //    var participants = await this._database.Query<MatchDayParticipant>()
            //        .Where(f => f.MatchDayId == matchDayId)
            //        .ToListAsync();

            //    if (participants.Any(f => f.Id == match.Participant1.Id) == false)
            //        return this.BadRequest();

            //    if (participants.Any(f => f.Id == match.Participant2.Id) == false)
            //        return this.BadRequest();

            //    var toSave = new Match
            //    {
            //        MatchDayId = matchDayId,
            //        //Number = match.Number,
            //        //CreatedByAccountId = 0,
            //        Participant1AccountId = match.Participant1.Id,
            //        Participant2AccountId = match.Participant2.Id,
            //        Participant1Points = match.Participant1Points,
            //        Participant2Points = match.Participant2Points,
            //        StartTime = match.StartTime,
            //        EndTime = match.EndTime
            //    };

            //    await this._database.InsertAsync(toSave);
            //    var result = await this._database.QueryAsync(MatchDTOQuery.For(toSave.Id));

            //    transaction.Complete();

            //    return this.Created(string.Empty, result);
            //}
        }

        [HttpPut]
        [Route("{matchDayId:int}/Matches/{matchId:int}")]
        public async Task<IActionResult> UpdateMatch(int matchDayId, int matchId, [FromBody] Match match, CancellationToken token)
        {
            throw new NotImplementedException();

            //using (var transaction = this._database.GetTransaction())
            //{
            //    var toUpdate = await this._database.SingleOrDefaultByIdAsync<Match>(matchId);

            //    if (toUpdate == null)
            //        return this.BadRequest();

            //    toUpdate.StartTime = match.StartTime;
            //    toUpdate.EndTime = match.EndTime;
            //    toUpdate.Participant1Points = match.Participant1Points;
            //    toUpdate.Participant2Points = match.Participant2Points;

            //    await this._database.UpdateAsync(toUpdate);

            //    var result = await this._database.QueryAsync(MatchDTOQuery.For(toUpdate.Id));

            //    transaction.Complete();

            //    return this.Ok(result);
            //}
        }
    }
}