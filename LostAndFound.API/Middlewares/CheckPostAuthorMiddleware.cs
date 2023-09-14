using LostAndFound.Core.Exceptions.Authenticate;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LostAndFound.API.Middlewares
{
    public class CheckPostAuthorMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckPostAuthorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IPostService service)
        {
            var postId = context.Request.Query["postId"];
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userId == null)
                throw new UnauthorizedException();

            if (String.IsNullOrEmpty(postId))
                throw new NoIdFoundException();
            
            if (!(await service.CheckPostAuthorAsync(Int32.Parse(postId), userId.Value)))
                throw new NotPermittedException("User doesn't have permitted to change this post");

            await _next(context);
        }
    }
}
