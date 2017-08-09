using MatchMaker.Api.AppSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MatchMaker.Api.Setup
{
    public static class Settings
    {
        public static void AddSettings(this IServiceCollection self, IConfiguration configuration)
        {
            self.Configure<DatabaseSettings>(configuration.GetSection("Database"));
            self.Configure<AccountSettings>(configuration.GetSection("Account"));
        }
    }
}