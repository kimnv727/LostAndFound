using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserMediaService _userMediaService;
        //TODO: add Student/LecturerCode field - wait for meeting
        public UserController(IUserService userService, IUserMediaService userMediaService)
        {
            _userService = userService;
            _userMediaService = userMediaService;
        }

        ///<summary>
        /// Get all users
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [QueryResponseCache(typeof(UserQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<UserDetailsReadDTO>))]
        public async Task<IActionResult> GetAll([FromQuery] UserQuery query)
        {
            var userPaginatedDto = await _userService.GetAllUsersAsync(query);

            return ResponseFactory.PaginatedOk(userPaginatedDto);
        }
        
        /// <summary>
        /// Get user's details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserDetailsReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetUserAsync(id);

            return ResponseFactory.Ok(user);
        }

        /// <summary>
        /// Get user's details by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserDetailsReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetUserByMail(string email)
        {
            var result = await _userService.GetUserByEmailAsync(email);

            return ResponseFactory.Ok(result);
        }

        ///<summary>
        /// Create new user
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<UserDetailsReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateUser(UserWriteDTO writeDTO)
        {
            var result = await _userService.CreateUserAsync(writeDTO);

            return ResponseFactory.CreatedAt(
                (nameof(GetUser)), 
                    nameof(UserController), 
                new { id = result.Id }, 
                result);
        }

        ///<summary>
        /// Update user password
        /// </summary>
        /// <remarks>Update user's password</remarks>
        /// <returns></returns>
        [HttpPatch("update-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateUserPasswordAsync(UserUpdatePasswordDTO updatePasswordDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            await _userService.UpdateUserPasswordAndSendEmailAsync(stringId, updatePasswordDTO);

            return ResponseFactory.NoContent();
        }
        
        /// <summary>
        /// Change user's IsActive value
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ChangeUserIsActiveStatus(string id)
        {
            await _userService.ChangeUserStatusAsync(id);

            return ResponseFactory.NoContent();
        }
        
        ///<summary>
        /// Update user information (authorized user)
        /// </summary>
        /// <remarks>Update user's information</remarks>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateUserDetailsAsync(UserUpdateDTO updateDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _userService.UpdateUserDetailsAsync(stringId, updateDTO);

            return ResponseFactory.Ok(result);
        }
        
        ///<summary>
        /// Update user information
        /// </summary>
        /// <remarks>Update user's information</remarks>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateUserDetailsAsync(string id, UserUpdateDTO updateDTO)
        {
            var user = await _userService.UpdateUserDetailsAsync(id, updateDTO);

            return ResponseFactory.Ok(user);
        }

        ///<summary>
        /// Update user avatar (this function will also add avatar if user currently don't have one)
        /// </summary>
        /// <remarks>Update user's avatar</remarks>
        /// <returns></returns>
        [HttpPatch("media")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateUserAvatarAsync(IFormFile avatar)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _userMediaService.UploadUserAvatar(avatar, stringId);

            return ResponseFactory.Ok(result);
        }
    }
}