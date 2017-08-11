using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Api.Databases;
using MatchMaker.Shared.Common;
using MatchMaker.Shared.MatchDays;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers.MatchDays
{
    [Route("MatchDays")]
    public class MatchDaysController : MatchMakerController
    {
        private readonly IDatabaseSession _databaseSession;

        public MatchDaysController(IDatabaseSession databaseSession)
        {
            Guard.NotNull(databaseSession, nameof(databaseSession));

            this._databaseSession = databaseSession;
        }

        [Authorize]
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