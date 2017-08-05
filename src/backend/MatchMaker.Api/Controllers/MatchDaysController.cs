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
    public class MatchDaysController : Controller
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public MatchDaysController(IDbConnectionFactory dbConnectionFactory)
        {
            this._dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewMatchDay([FromBody]CreateMatchDayData data, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (data == null || data.ParticipantIds == null || data.ParticipantIds.Count == 0)
                return this.BadRequest();

            using (var connection = this._dbConnectionFactory.Create())
            using (var transaction = connection.BeginTransaction())
            {
                int matchDayId = await connection.CreateMatchDay(data.When, data.ParticipantIds, transaction, cancellationToken);
                var matchDayCompact = await connection.GetMatchDayCompact(matchDayId, transaction, cancellationToken);

                transaction.Commit();

                return this.Created(string.Empty, matchDayCompact);
            }
        }

    }
}