using System;
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
    public class MatchDaysController : Controller
    {
        private readonly IDatabase _database;

        public MatchDaysController(IDatabase database)
        {
            this._database = database ?? throw new ArgumentNullException(nameof(database));
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewMatchDay([FromBody]CreateMatchDayData data, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (data == null || data.ParticipantIds == null || data.ParticipantIds.Count == 0)
                return this.BadRequest();
            
            using (var transaction = this._database.GetTransaction())
            {
                var matchday = new MatchDay
                {
                    When = data.When
                };
                await this._database.InsertAsync(matchday);

                foreach (var participantId in data.ParticipantIds)
                {
                    var participant = new MatchDayParticipant
                    {
                        MatchDayId = matchday.Id,
                        AccountId = participantId
                    };

                    await this._database.InsertAsync(participant);
                }

                var result = await this._database.QueryAsync(MatchDayCompactDTOQuery.For(matchday.Id));

                transaction.Complete();

                return this.Created(string.Empty, result);
            }
        }

    }
}