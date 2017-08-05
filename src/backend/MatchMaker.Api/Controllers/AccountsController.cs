using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using JWT.Algorithms;
using MatchMaker.Api.AppSettings;
using MatchMaker.Api.Database;
using MatchMaker.Api.Entities;
using MatchMaker.Api.Services;
using MatchMaker.Shared.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MatchMaker.Api.Controllers
{
    [Route("accounts")]
    public class AccountsController : Controller
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IJwtService _jwtService;

        public AccountsController(IDbConnectionFactory dbConnectionFactory, IJwtService jwtService)
        {
            this._dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
            this._jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterData data, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (data == null || string.IsNullOrWhiteSpace(data.EmailAddress) || string.IsNullOrWhiteSpace(data.Password))
                return this.BadRequest();

            data.EmailAddress = data.EmailAddress.Trim();

            using (var connection = this._dbConnectionFactory.Create())
            using (var transaction = connection.BeginTransaction())
            {
                var existingAccount = await connection.QueryAccountByEmail(data.EmailAddress, transaction, cancellationToken);

                if (existingAccount != null)
                    return this.BadRequest("Email address is already in use.");

                var account = new Account
                {
                    EmailAddress = data.EmailAddress,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(data.Password),
                };

                await connection.CreateAccount(account, transaction, cancellationToken);

                transaction.Commit();
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

            using (var connection = this._dbConnectionFactory.Create())
            using (var transaction = connection.BeginTransaction())
            {
                var account = await connection.QueryAccountByEmail(data.EmailAddress, transaction, cancellationToken);

                if (account == null)
                    return this.NotFound();

                if (BCrypt.Net.BCrypt.Verify(data.Password, account.PasswordHash) == false)
                    return this.Unauthorized();

                var result = new LoginResult
                {
                    Token = this._jwtService.Create(account.Id, account.EmailAddress),
                };

                transaction.Commit();

                return this.Ok(result);
            }
        }
    }
}