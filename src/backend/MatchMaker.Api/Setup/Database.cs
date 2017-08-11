using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using MatchMaker.Api.AppSettings;
using MatchMaker.Api.Databases;
using MatchMaker.Api.Databases.Repositories.Accounts;
using MatchMaker.Api.Databases.Repositories.MatchDays;
using MatchMaker.Api.Databases.Repositories.Matches;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MatchMaker.Api.Setup
{
    public static class Database
    {
        public static void AddDatabase(this IServiceCollection self, IConfiguration configuration)
        {
            self.AddScoped<IDbConnection>(f =>
            {
                var settings = f.GetService<IOptions<DatabaseSettings>>();
                var connection = new SqlConnection(settings.Value.ConnectionString);

                connection.Open();

                return connection;
            });

            self.AddScoped<IDbTransaction>(f =>
            {
                var connection = f.GetService<IDbConnection>();
                return connection.BeginTransaction();
            });

            self.AddScoped<IDatabaseSession, DatabaseSession>();
            self.AddScoped<IAccountRepository, AccountRepository>();
            self.AddScoped<IMatchDayRepository, MatchDayRepository>();
            self.AddScoped<IMatchRepository, MatchRepository>();
        }

        public static void UseAutoCommit(this IApplicationBuilder self)
        {
            self.Use(async (context, next) =>
            {
                bool error = false;
                try
                {
                    await next();
                }
                catch (Exception exception)
                {
                    if (Debugger.IsAttached)
                        Debugger.Break();

                    error = true;
                }

                if (error == false)
                    context.RequestServices.GetService<IDatabaseSession>().Commit();
            });
        }
    }
}