using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Simple.Web.Server.Middleware;

namespace Simple.Web.Server.Extensions
{
    public static class DowntimeMiddlewareExtensions
    {
        public static IApplicationBuilder UseDowntime(this IApplicationBuilder app, IConfiguration configuration)
        {
            return app.Use(next => new DowntimeMiddleware(next, configuration).Invoke);
        }
    }
}