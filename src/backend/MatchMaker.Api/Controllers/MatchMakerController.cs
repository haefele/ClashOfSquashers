using System.Linq;
using MatchMaker.Api.Services.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers
{
    public abstract class MatchMakerController : Controller
    {
        private AuthenticatedAccount _user;

        public new AuthenticatedAccount User
        {
            get
            {
                if (this._user != null)
                    return this._user;

                if (base.User == null)
                    return null;

                var accountIdClaim = base.User.Claims.First(f => f.Type == JwtClaims.AccountId);
                var emailAddressClaim = base.User.Claims.First(f => f.Type == JwtClaims.EmailAddress);

                return this._user = new AuthenticatedAccount(int.Parse(accountIdClaim.Value), emailAddressClaim.Value);
            }
        }
    }
}