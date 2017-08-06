using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Api.Databases.Queries;
using MatchMaker.Api.Entities;
using MatchMaker.Shared.MatchDays;
using Microsoft.AspNetCore.Mvc;
using NPoco;

namespace MatchMaker.Api.Controllers
{
    [Route("MatchDays")]
    public class MatchesController : Controller
    {
        private readonly IDatabase _database;

        public MatchesController(IDatabase database)
        {
            this._database = database ?? throw new ArgumentNullException(nameof(database));
        }

        [HttpGet]
        [Route("{matchDayId:int}/Matches/Next")]
        public async Task<IActionResult> GetNextMatch(int matchDayId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (matchDayId <= 0)
                return this.BadRequest();
            
            using (var transaction = this._database.GetTransaction())
            {
                var matchDay = await this._database.SingleOrDefaultByIdAsync<MatchDay>(matchDayId);

                if (matchDay == null)
                    return this.NotFound();

                var matches = await this._database.Query<Match>().Where(f => f.MatchDayId == matchDayId).ToListAsync();
                var participants = await this._database.Query<MatchDayParticipant>().Where(f => f.MatchDayId == matchDayId).ToListAsync();

                if (participants.Count == 0)
                    return this.BadRequest();

                var uniqueMatches = this.CreateUniqueMatches(participants);
                foreach (var match in matches)
                {
                    var uniqueMatch = uniqueMatches.FirstOrDefault(f =>
                        f.Participant1.Id == match.Participant1AccountId && f.Participant2.Id == match.Participant2AccountId ||
                        f.Participant1.Id == match.Participant2AccountId && f.Participant2.Id == match.Participant1AccountId);

                    uniqueMatch.Count++;
                }

                var nextMatch = uniqueMatches.FirstOrDefault(f => f.Count == uniqueMatches.Select(d => d.Count).Min());
                
                var result = new MatchDTO
                {
                    Id = 0,
                    Number = matches.Any() 
                        ? matches.Max(f => f.Number) + 1 
                        : 1,
                    MatchDayId = matchDayId,
                    Participant1 = await this._database.QueryAsync(AccountCompactDTOQuery.For(nextMatch.Participant1.AccountId)),
                    Participant2 = await this._database.QueryAsync(AccountCompactDTOQuery.For(nextMatch.Participant2.AccountId)),
                    CreatedBy = null,
                    Participant1Points = 0,
                    Participant2Points = 0,
                    StartTime = null,
                    EndTime = null
                };

                transaction.Complete();

                return this.Ok(result);
            }
        }

        [HttpPost]
        [Route("{matchDayId:int}/Matches")]
        public async Task<IActionResult> SaveMatch(int matchDayId, [FromBody] MatchDTO match, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (matchDayId <= 0 || match == null || match.Participant1 == null || match.Participant2 == null)
                return this.BadRequest();
            
            using (var transaction = this._database.GetTransaction())
            {
                var participants = await this._database.Query<MatchDayParticipant>()
                    .Where(f => f.MatchDayId == matchDayId)
                    .ToListAsync();

                if (participants.Any(f => f.Id == match.Participant1.Id) == false)
                    return this.BadRequest();

                if (participants.Any(f => f.Id == match.Participant2.Id) == false)
                    return this.BadRequest();

                var toSave = new Match
                {
                    MatchDayId = matchDayId,
                    //Number = match.Number,
                    //CreatedByAccountId = 0,
                    Participant1AccountId = match.Participant1.Id,
                    Participant2AccountId = match.Participant2.Id,
                    Participant1Points = match.Participant1Points,
                    Participant2Points = match.Participant2Points,
                    StartTime = match.StartTime,
                    EndTime = match.EndTime
                };

                await this._database.InsertAsync(toSave);
                var result = await this._database.QueryAsync(MatchDTOQuery.For(toSave.Id));

                transaction.Complete();

                return this.Created(string.Empty, result);
            }
        }

        [HttpPut]
        [Route("{matchDayId:int}/Matches/{matchId:int}")]
        public async Task<IActionResult> UpdateMatch(int matchDayId, int matchId, [FromBody] MatchDTO match, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var transaction = this._database.GetTransaction())
            {
                var toUpdate = await this._database.SingleOrDefaultByIdAsync<Match>(matchId);

                if (toUpdate == null)
                    return this.BadRequest();

                toUpdate.StartTime = match.StartTime;
                toUpdate.EndTime = match.EndTime;
                toUpdate.Participant1Points = match.Participant1Points;
                toUpdate.Participant2Points = match.Participant2Points;

                await this._database.UpdateAsync(toUpdate);

                var result = await this._database.QueryAsync(MatchDTOQuery.For(toUpdate.Id));

                transaction.Complete();

                return this.Ok(result);
            }
        }

        #region Private Methods
        private List<Matchup> CreateUniqueMatches(List<MatchDayParticipant> participants)
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
            public MatchDayParticipant Participant1 { get; set; }
            public MatchDayParticipant Participant2 { get; set; }
            public int Count { get; set; }
        }
        #endregion
    }
}