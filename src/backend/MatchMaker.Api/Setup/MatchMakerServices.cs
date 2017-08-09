using MatchMaker.Api.Databases;
using MatchMaker.Api.Services.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace MatchMaker.Api.Setup
{
    public static class MatchMakerServices
    {
        public static void AddMatchMakerServices(this IServiceCollection self)
        {
            self.AddTransient<IJwtService, JwtService>();
            self.AddTransient<IPasswordHasher, BCryptPasswordHasher>();
        }
    }
}