using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.API.Filters;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.Authenticate;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.API.Controllers
{
   [Route("auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        //private readonly IAuthenService _authService;
        //private readonly IUserService _userService;
        private readonly IFirebaseAuthService _authService;

        public AuthenticateController(IFirebaseAuthService authService)
        {
            _authService = authService;
        }

        /*/// <summary>
        /// Authenticate
        /// </summary>
        /// <returns>UserDetailsReadDTO</returns>
        [HttpGet("authenticate")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserDetailAuthenticateReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> Authenticate()
        {
            return ResponseFactory.Ok(await _authService.Authenticate(
                new Guid(User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value))
            );
        }*/

        /// <summary>
        /// Login with email and password
        /// </summary>
        /// <returns>LoginResponseDTO</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LoginResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var result = await _authService.Login(loginRequest);
            return ResponseFactory.Ok(result);
        }
        
        /*/// <summary>
        /// Login refresh token
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LoginRequestDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> Refresh([FromBody] string refreshTokenValue)
        {
            var result = await _authService.LoginWithRefreshToken(refreshTokenValue);

            return ResponseFactory.Ok(result);
        }*/

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDTO logoutRequest)
        {
            await _authService.Logout();

            return ResponseFactory.NoContent();
        }
    }
}