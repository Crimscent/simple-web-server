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
        }

        public IConfigurationRoot Configuration { get; }

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
                    var response = $@"{{""version"":""{Configuration[AppSettingsConst.Version]}""}}";
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    context.Response.ContentType = "text/json";
                    context.Response.ContentLength = response.Length;

                    await context.Response.WriteAsync(response);
                });
            });

            app.UseDowntime(Configuration);
            app.UseStaticFiles();
            app.UseIndex();
            app.UseStaticFiles();
        }
    }
}
