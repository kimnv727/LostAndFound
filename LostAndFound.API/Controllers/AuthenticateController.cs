using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.API.Filters;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.Authenticate;
using LostAndFound.Infrastructure.DTOs.User;
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
        private readonly IFirebaseAuthService _authService;
        private readonly IUserService _userService;
        //TODO: get and store device token
        public AuthenticateController(IFirebaseAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <returns>UserDetailsReadDTO</returns>
        [Authorize]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserDetailAuthenticateReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> Authenticate()
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _userService.GetUserAsync(stringId);
            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Google Login Authenticate
        /// </summary>
        /// <returns>UserDetailsReadDTO</returns>
        [AllowAnonymous]
        [HttpPost("googleLoginAuthenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserDetailAuthenticateReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> GoogleLoginAuthenticate([FromBody] AuthenticateDTO authenticateRequest)
        {
            var result = await _authService.Authenticate(authenticateRequest.Uid, authenticateRequest.Email, 
                authenticateRequest.Name, authenticateRequest.Avatar, authenticateRequest.Phone);

            return ResponseFactory.Ok(result);
        }
        
        /// <summary>
        /// Login with email and password for Manager + Admin
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