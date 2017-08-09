using System;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Api.Databases;
using MatchMaker.Shared.MatchDays;
using Microsoft.AspNetCore.Mvc;
using NPoco;

namespace MatchMaker.Api.Controllers
{
    [Route("MatchDays")]
    public class MatchDaysController : Controller
    {
        private readonly IDatabaseSession _databaseSession;

        public MatchDaysController(IDatabaseSession databaseSession)
        {
            this._databaseSession = databaseSession;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewMatchDay([FromBody]CreateMatchDayData data, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (data == null || data.ParticipantIds == null || data.ParticipantIds.Count == 0)
                return this.BadRequest();

            var matchDay = await this._databaseSession.MatchDayRepository.CreateNewAsync(data.When, data.ParticipantIds, cancellationToken);
            return this.Created(string.Empty, matchDay);
        }
    }
}