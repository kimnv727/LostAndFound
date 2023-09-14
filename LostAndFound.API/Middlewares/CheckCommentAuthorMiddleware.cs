using LostAndFound.Core.Exceptions.Authenticate;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Security.Claims;

namespace LostAndFound.API.Middlewares
{
    public class CheckCommentAuthorMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckCommentAuthorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICommentService service)
        {
            var commentId = context.Request.Query["commentId"];
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var method = context.Request.Method.ToString();
            if (userId == null)
                throw new UnauthorizedException();

            if (String.IsNullOrEmpty(commentId))
                throw new NoIdFoundException();

            if (!(await service.CheckCommentAuthorAsync(Int32.Parse(commentId), userId.Value)))
                throw new NotPermittedException("User doesn't have permitted to change this comment");

            await _next(context);
        }
    }
}
