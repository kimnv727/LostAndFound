using LostAndFound.Core.Exceptions.Authenticate;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Security.Claims;
using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.API.Middlewares
{
    public class CheckItemFounderMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckItemFounderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IItemService service)
        {
            var itemId = context.Request.Query["itemId"];
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var method = context.Request.Method.ToString();
            if (userId == null)
                throw new UnauthorizedException();

            if (String.IsNullOrEmpty(itemId))
                throw new NoIdFoundException();

            if (!(await service.CheckItemFounderAsync(Int32.Parse(itemId), userId.Value)))
                throw new NotPermittedException("User doesn't have permitted to change this item");

            await _next(context);
        }
    }
}
