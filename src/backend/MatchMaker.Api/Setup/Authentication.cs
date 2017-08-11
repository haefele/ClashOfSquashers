using System.Text;
using MatchMaker.Api.AppSettings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MatchMaker.Api.Setup
{
    public static class Authentication
    {
        public static void UseAuthentication(this IApplicationBuilder self)
        {
            var settings = self.ApplicationServices.GetService<IOptions<AccountSettings>>();

            self.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = false,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Value.JwtSecret))
                }
            });
        }
    }
}