using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            app.Map("/ping", subApp =>
            {
                subApp.Run(async context =>
                {
                    var response = $@"{{""version"":""{Configuration["Version"]}""}}";
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    context.Response.ContentType = "text/json";
                    context.Response.ContentLength = response.Length;

                    await context.Response.WriteAsync(response);
                });
            });

            app.UseStaticFiles();

            app.Use(next =>
            {
                return (context =>
                {
                    context.Request.Path = "/index.html";
                    return next(context);
                });
            });

            app.UseStaticFiles();
        }
    }
}
