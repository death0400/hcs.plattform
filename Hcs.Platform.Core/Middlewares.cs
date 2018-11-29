using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Middlewares
    {
        public static async Task AngularRouteCheckMiddleware(HttpContext context, Func<Task> next)
        {
            await next.Invoke();
            if (context.Response.StatusCode == 404 && !context.Request.Path.Value.StartsWith("/api"))
            {
                context.Request.Path = new AspNetCore.Http.PathString("/index.html");
                await next.Invoke();
            }
        }
        public static async Task PlatformControllerMiddleware(HttpContext context, Func<Task> next)
        {
            var entityApiPrefix = "/api/entity/";
            if (context.Request.Path.Value.StartsWith(entityApiPrefix))
            {
                var cleanPath = context.Request.Path.Value.Substring(context.Request.Path.Value.IndexOf(entityApiPrefix) + entityApiPrefix.Length);
                var pathpart = cleanPath.Split('/');
                var key = pathpart[0];
                var method = context.Request.Method;
                if (method.ToLowerInvariant() == "get")
                {
                    if (pathpart.Length == 1)
                    {
                        method = "query";
                    }
                }
                var controllerName = Hcs.Platform.Core.PlatformModule.EntityApiContextContainer.GetController(key, method);
                if (controllerName == null)
                {
                    context.Response.StatusCode = 404;
                }
                else
                {
                    context.Items.Add("apiKey", key);
                    context.Request.Path = new AspNetCore.Http.PathString($"{entityApiPrefix}{controllerName}/{string.Join("/", pathpart.Skip(1))}".TrimEnd('/'));
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }

}