using System;
using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Api.Entities;
using MatchMaker.Api.Services.Jwt;
using MatchMaker.Shared.Accounts;
using Microsoft.AspNetCore.Mvc;
using NPoco;

namespace MatchMaker.Api.Controllers
{
    [Route("Accounts")]
    public class AccountsController : Controller
    {
        private readonly IDatabase _database;
        private readonly IJwtService _jwtService;

        public AccountsController(IDatabase database, IJwtService jwtService)
        {
            this._database = database ?? throw new ArgumentNullException(nameof(database));
            this._jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterData data, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (data == null || string.IsNullOrWhiteSpace(data.EmailAddress) || string.IsNullOrWhiteSpace(data.Password))
                return this.BadRequest();

            data.EmailAddress = data.EmailAddress.Trim();
            
            using (var transaction = this._database.GetTransaction())
            {
                var existingAccount = await this._database.Query<Account>()
                    .Where(f => f.EmailAddress == data.EmailAddress)
                    .FirstOrDefaultAsync();
                
                if (existingAccount != null)
                    return this.BadRequest("Email address is already in use.");

                var account = new Account
                {
                    EmailAddress = data.EmailAddress,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(data.Password),
                };

                await this._database.InsertAsync(account);

                transaction.Complete();
            }

            return this.Ok();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginData data, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (data == null || string.IsNullOrWhiteSpace(data.EmailAddress) || string.IsNullOrWhiteSpace(data.Password))
                return this.BadRequest();

            data.EmailAddress = data.EmailAddress.Trim();
            
            using (var transaction = this._database.GetTransaction())
            {
                var account = await this._database.Query<Account>()
                    .Where(f => f.EmailAddress == data.EmailAddress)
                    .FirstOrDefaultAsync();

                if (account == null)
                    return this.NotFound();

                if (BCrypt.Net.BCrypt.Verify(data.Password, account.PasswordHash) == false)
                    return this.Unauthorized();

                var result = new LoginResult
                {
                    Token = this._jwtService.Create(account.Id, account.EmailAddress),
                };

                transaction.Complete();

                return this.Ok(result);
            }
        }
    }
}