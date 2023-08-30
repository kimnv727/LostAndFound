using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using LostAndFound.Core.Exceptions.authenticate;
using System.Security.Claims;

namespace LostAndFound.API.Middlewares
{
    public class AuthenticateMiddelware
    {
        private readonly RequestDelegate _next;

        public AuthenticateMiddelware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userId == null)
                throw new UnauthorizedException();

            //TODO: Replace with token check function here
            if (userId.Value != "IwZdGow330VpjeBFnrIm7T5H0262")
                throw new UnauthorizedException();

            await _next(context);
        }
    }
}
