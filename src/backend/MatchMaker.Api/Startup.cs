using System.Threading.Tasks;
using MatchMaker.Api.Databases;
using MatchMaker.Api.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            services.AddSettings(this.Configuration);
            services.AddDatabase(this.Configuration);
            services.AddMatchMakerServices();
            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.Use((context, next) =>
            {
                var session = context.RequestServices.GetService<IDatabaseSession>();

                bool error = false;
                try
                {
                    next();
                }
                catch
                {
                    error = true;
                }

                if (error == false)
                    session.Commit();

                return Task.CompletedTask;
            });

            app.UseMvc();
        }
    }
}
