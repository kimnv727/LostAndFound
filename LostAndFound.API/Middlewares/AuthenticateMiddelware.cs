using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using LostAndFound.Core.Exceptions.Authenticate;
using System.Security.Claims;
using System;
using LostAndFound.Infrastructure.Services.Interfaces;

namespace LostAndFound.API.Middlewares
{
    public class AuthenticateMiddelware
    {
        private readonly RequestDelegate _next;

        public AuthenticateMiddelware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserService service)
        {
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            
            if (userId == null)
                throw new UnauthorizedException();

            if (!(await service.CheckUserExisted(userId.Value)))
                throw new UnauthorizedException();

            await _next(context);
        }
    }
}
