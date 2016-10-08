using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Simple.Web.Server.Middleware
{
    public class DowntimeMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IConfiguration _configuration;

        public DowntimeMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _next = next;
            _configuration = configuration;
        }

        public Task Invoke(HttpContext context)
        {
            bool isDowntime;
            if (bool.TryParse(_configuration[AppSettingsConst.IsDowntime], out isDowntime) && isDowntime)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return Task.CompletedTask;
            }

            return _next(context);
        }
    }
}