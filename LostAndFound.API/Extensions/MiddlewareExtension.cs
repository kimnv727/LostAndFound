using LostAndFound.API.Middlewares;
using Microsoft.AspNetCore.Builder;

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
            app.UseWhen(context => !context.Request.Path.StartsWithSegments("/auth") 
                        || context.Request.Path.StartsWithSegments("/auth/logout"),
                appBuilder =>
                {
                    appBuilder.UseMiddleware<AuthenticateMiddelware>();
                });
        }
    }
}
