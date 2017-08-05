using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MatchMaker.Api.AppSettings;
using MatchMaker.Shared.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MatchMaker.Api.Controllers
{
    [Route("accounts")]
    public class AccountsController : Controller
    {
        private readonly IOptions<Auth0Settings> _auth0Settings;

        public AccountsController(IOptions<Auth0Settings> auth0Settings)
        {
            this._auth0Settings = auth0Settings ?? throw new ArgumentNullException(nameof(auth0Settings));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterData data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.EmailAddress) || string.IsNullOrWhiteSpace(data.Password))
                return this.BadRequest();

            var client = new HttpClient();
            client.BaseAddress = new Uri($"https://{this._auth0Settings.Value.Domain}.auth0.com");
            
            var request = new HttpRequestMessage(HttpMethod.Post, "oauth/ro");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = this._auth0Settings.Value.ClientId,
                ["username"] = data.EmailAddress,
                ["password"] = data.Password,
                ["connection"] = this._auth0Settings.Value.UsernamePasswordConnectionName,
                ["grant_type"] = "password",
                ["scope"] = "openid"
            });

            var response = await client.SendAsync(request);

            return this.Ok();
        }
    }
}