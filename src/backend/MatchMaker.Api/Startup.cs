using System.Data.SqlClient;
using MatchMaker.Api.AppSettings;
using MatchMaker.Api.Entities;
using MatchMaker.Api.Services.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NPoco;
using NPoco.FluentMappings;

namespace MatchMaker.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DatabaseSettings>(this.Configuration.GetSection("Database"));
            services.Configure<AccountSettings>(this.Configuration.GetSection("Account"));

            var factory = DatabaseFactory.Config(f =>
            {
                f.UsingDatabase(() => new Database(
                    this.Configuration.GetSection("Database").GetValue<string>("ConnectionString"),
                    DatabaseType.SqlServer2012,
                    SqlClientFactory.Instance));
                f.WithFluentConfig(FluentMappingConfiguration.Configure(
                    new AccountMaps(),
                    new MatchDayMaps(),
                    new MatchDayParticipantMaps(),
                    new MatchMaps()));
            });

            services.AddSingleton<DatabaseFactory>(factory);
            services.AddSingleton<IDatabase>(f => f.GetService<DatabaseFactory>().GetDatabase());
            services.AddSingleton<IJwtService, JwtService>();
            
            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
