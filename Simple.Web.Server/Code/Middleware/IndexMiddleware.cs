using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Simple.Web.Server.Middleware
{
    public class IndexMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly Func<HttpContext, bool> _predicate;

        public IndexMiddleware(RequestDelegate next, Func<HttpContext, bool> predicate)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _next = next;
            _predicate = predicate;
        }

        public Task Invoke(HttpContext context)
        {
            if (_predicate(context))
            {
                context.Request.Path = "/index.html";
            }

            return _next(context);
        }
    }
}
