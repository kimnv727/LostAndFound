using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.API.Filters;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.Authenticate;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.DTOs.UserDevice;
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
        private readonly IUserDeviceService _userDeviceService;
        //TODO: check soft deleted users wont be able to login
        public AuthenticateController(IFirebaseAuthService authService, IUserService userService, IUserDeviceService userDeviceService)
        {
            _authService = authService;
            _userService = userService;
            _userDeviceService = userDeviceService;
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="userDeviceToken"></param>
        /// <returns>UserDetailsReadDTO</returns>
        [Authorize]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserDetailAuthenticateReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> Authenticate([Required] string userDeviceToken)
        {
            //check user existed and return role
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _userService.GetUserAsync(stringId);
            
            //check user device existed -> if not create new
            var userDevice = await _userDeviceService.GetUserDeviceByTokenAsync(userDeviceToken);
            if (userDevice == null)
            {
                var userDeviceWriteDTO = new UserDeviceWriteDTO
                {
                    Token = userDeviceToken,
                    UserId = stringId
                };
                await _userDeviceService.CreateUserDevice(userDeviceWriteDTO);
            }
            
            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Google Login Authenticate
        /// </summary>
        /// <param name="userDeviceToken"></param>
        /// <returns>UserDetailsReadDTO</returns>
        [AllowAnonymous]
        [HttpPost("googleLoginAuthenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserDetailAuthenticateReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> GoogleLoginAuthenticate([FromBody] AuthenticateDTO authenticateRequest)
        {
            //create new User for Google Login
            var result = await _authService.Authenticate(authenticateRequest.Uid, authenticateRequest.Email, 
                authenticateRequest.Name, authenticateRequest.Avatar, authenticateRequest.Phone);

            //check user device existed -> if not create new
            var userDevice = await _userDeviceService.GetUserDeviceByTokenAsync(authenticateRequest.DeviceToken);
            if (userDevice == null)
            {
                var userDeviceWriteDTO = new UserDeviceWriteDTO
                {
                    Token = authenticateRequest.DeviceToken,
                    UserId = authenticateRequest.Uid
                };
                await _userDeviceService.CreateUserDevice(userDeviceWriteDTO);
            }
            
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
        
        /// <summary>
        /// Login with Refresh Token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAccessTokenWithRefreshToken([Required]string refreshToken)
        {
            return ResponseFactory.Ok(await _authService.GetAccessTokenWithRefreshToken(refreshToken));
        }
    }
}