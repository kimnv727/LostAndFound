using LostAndFound.API.Middlewares;
using Microsoft.AspNetCore.Builder;
using System.Net.Http;

namespace LostAndFound.API.Extensions
{
    public static class MiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        public static void UseTokenCheckMiddleware(this IApplicationBuilder app)
        {
            app.UseWhen(context => (!context.Request.Path.StartsWithSegments("/auth"))
                        || context.Request.Path.StartsWithSegments("/auth/logout")
                        ,
                appBuilder =>
                {
                    appBuilder.UseMiddleware<AuthenticateMiddelware>();
                });
        }

        public static void UseCheckPostAuthorMiddleware(this IApplicationBuilder app)
        {
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/posts")
                        && !context.Request.Method.Equals("PATCH")
                        && !context.Request.Method.Equals("GET")
                        && !context.Request.Method.Equals("POST")
                        ,
                appBuilder =>
                {
                    appBuilder.UseMiddleware<CheckPostAuthorMiddleware>();
                });
        }

        public static void UseCheckItemFounderMiddleware(this IApplicationBuilder app)
        {
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/items")
                        && !context.Request.Method.Equals("GET")
                        && !context.Request.Method.Equals("PATCH")
                        && !context.Request.Method.Equals("POST")
                        ,
            appBuilder =>
            {
                appBuilder.UseMiddleware<CheckItemFounderMiddleware>();
            });
        }

        public static void UseCheckCommentAuthorMiddleware(this IApplicationBuilder app)
        {
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/comments")
                        && !context.Request.Method.Equals("GET")
                        && !context.Request.Method.Equals("PATCH")
                        && !context.Request.Method.Equals("POST")
                        ,
            appBuilder =>
            {
                appBuilder.UseMiddleware<CheckCommentAuthorMiddleware>();
            });
        }
    }
}
