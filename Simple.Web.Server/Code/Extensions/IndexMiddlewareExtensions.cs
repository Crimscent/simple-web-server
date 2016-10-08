using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Simple.Web.Server.Middleware;

namespace Simple.Web.Server.Extensions
{
    public static class IndexMiddlewareExtensions
    {
        public static IApplicationBuilder UseIndex(this IApplicationBuilder app)
        {
            return app.UseIndexWhen(context => true);
        }

        public static IApplicationBuilder UseIndexWhen(this IApplicationBuilder app, Func<HttpContext, bool> predicate)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return app.Use(next => new IndexMiddleware(next, predicate).Invoke);
        }
    }
}