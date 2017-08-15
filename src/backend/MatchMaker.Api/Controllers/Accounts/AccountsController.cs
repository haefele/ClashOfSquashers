using System.Threading;
using System.Threading.Tasks;
using MatchMaker.Api.Databases;
using MatchMaker.Api.Services.Jwt;
using MatchMaker.Api.Services.PasswordHasher;
using MatchMaker.Shared.Accounts;
using MatchMaker.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers.Accounts
{
    [Route("Accounts")]
    public class AccountsController : MatchMakerController
    {
        private readonly IDatabaseSession _databaseSession;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public AccountsController(IDatabaseSession databaseSession, IJwtService jwtService, IPasswordHasher passwordHasher)
        {
            Guard.NotNull(databaseSession, nameof(databaseSession));
            Guard.NotNull(jwtService, nameof(jwtService));
            Guard.NotNull(passwordHasher, nameof(passwordHasher));

            this._databaseSession = databaseSession;
            this._jwtService = jwtService;
            this._passwordHasher = passwordHasher;
        }

        [Authorize]
        [HttpGet]
        [Route("{accountId:int}")]
        public async Task<IActionResult> GetById(int accountId, CancellationToken token)
        {
            if (accountId <= 0)
                return this.BadRequest();

            var account = await this._databaseSession.AccountRepository.GetAccountByIdAsync(accountId, token);

            if (account == null)
                return this.NotFound();

            return this.Ok(account);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery]string searchText, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return this.BadRequest();

            var accounts = await this._databaseSession.AccountRepository.SearchAccountsAsync(searchText, token);
            return this.Ok(accounts);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterData data, CancellationToken token)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.EmailAddress) || string.IsNullOrWhiteSpace(data.Password))
                return this.BadRequest();

            await this._databaseSession.AccountRepository.CreateAsync(data.EmailAddress, data.Password, token);

            return this.Ok();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginData data, CancellationToken token)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.EmailAddress) || string.IsNullOrWhiteSpace(data.Password))
                return this.BadRequest();
            
            (Account account, string passwordHash) found = await this._databaseSession.AccountRepository.GetAccountAndPasswordHashFromEmailAddressAsync(data.EmailAddress, token);

            if (found.account == null)
                return this.NotFound();

            if (this._passwordHasher.Validate(found.passwordHash, data.Password) == false)
                return this.Unauthorized();

            var result = new LoginResult
            {
                Token = this._jwtService.Create(found.account.Id, found.account.EmailAddress),
            };

            return this.Ok(result);
        }
    }
}