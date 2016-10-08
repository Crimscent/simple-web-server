using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Simple.Web.Server.Extensions;

namespace Simple.Web.Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();

            Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        }

        public IConfigurationRoot Configuration { get; }

        public string Version { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            app.Map("/ping", subApp =>
            {
                subApp.UseDowntime(Configuration);
                subApp.Run(async context =>
                {
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/json";

                    await context.Response.WriteAsync($@"{{""version"":""{Version}""}}");
                });
            });

            app.UseDowntime(Configuration);
            app.UseStaticFiles();
            app.UseIndex();
            app.UseStaticFiles();
        }
    }
}
