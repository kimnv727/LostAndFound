using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.API.Attributes;
using LostAndFound.API.Extensions;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.DTOs.UserMedia;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.API.Controllers
{
    //TODO: IMPORTANT -> Update user Name and avatar on firebase after update profile on front
    //TODO: add check Campus when login with email and password -> still have campus when login -> if new then use it to create new User -> If not then use it to check
    //TODO: also add propertyId when create new manager
    //TODO: also trim SchoolId to add when create
    //TODO: add check cho isActive/Campus -> add into Autheticate (check inside here)
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserMediaService _userMediaService;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly FirestoreProvider _firestoreProvider;
        private readonly INotificationService _notificationService;
        private readonly IUserDeviceService _userDeviceService;

        public UserController(IUserService userService, IUserMediaService userMediaService, IFirebaseAuthService firebaseAuthService
            , FirestoreProvider firestoreProvider, INotificationService notificationService, IUserDeviceService userDeviceService)
        {
            _userService = userService;
            _userMediaService = userMediaService;
            _firebaseAuthService = firebaseAuthService;
            _firestoreProvider = firestoreProvider;
            _notificationService = notificationService;
            _userDeviceService = userDeviceService;
        }

        ///<summary>
        /// Get all users
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<UserDetailsReadDTO>))]
        public async Task<IActionResult> GetAll([FromQuery] UserQuery query)
        {
            var userPaginatedDto = await _userService.GetAllUsersAsync(query);

            return ResponseFactory.PaginatedOk(userPaginatedDto);
        }
        
        ///<summary>
        /// Get all users ignore status
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-ignore-status")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<UserDetailsReadDTO>))]
        public async Task<IActionResult> GetAllIgnoreStatus([FromQuery] UserQueryIgnoreStatus query)
        {
            var userPaginatedDto = await _userService.GetAllUsersIgnoreStatusAsync(query);

            return ResponseFactory.PaginatedOk(userPaginatedDto);
        }

        ///<summary>
        /// Get all users ignore status exclude waiting verified
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-ignore-status-exclude")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<UserDetailsReadDTO>))]
        public async Task<IActionResult> GetAllIgnoreStatusExcludeWaitingVerified([FromQuery] UserQueryIgnoreStatusWithoutWaitingVerified query)
        {
            var userPaginatedDto = await _userService.GetAllUsersIgnoreStatusWithoutWaitingVerifiedAsync(query);

            return ResponseFactory.PaginatedOk(userPaginatedDto);
        }

        /// <summary>
        /// Get user's details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserDetailsReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetUser([Required] string id)
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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserDetailsReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetUserByMail([Required] string email)
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
        public async Task<IActionResult> CreateUser([Required] UserWriteDTO writeDTO)
        {
            var result = await _userService.CreateUserAsync(writeDTO);
            //create on Firebase
            await _firestoreProvider.CreateNewUser(writeDTO.FirstName + " " + writeDTO.LastName, writeDTO.Email, writeDTO.Avatar, result.Id);

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
        public async Task<IActionResult> UpdateUserPasswordAsync([Required] UserUpdatePasswordDTO updatePasswordDTO)
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<UserDetailsReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ChangeUserIsActiveStatus([Required] string id)
        {
            var result = await _userService.ChangeUserStatusAsync(id);

            return ResponseFactory.Ok(result);
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
        public async Task<IActionResult> UpdateUserDetailsAsync([Required]UserUpdateDTO updateDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _userService.UpdateUserDetailsAsync(stringId, updateDTO);

            var check = await _firestoreProvider.GetUser(stringId);
            if (check)
            {
                await _firestoreProvider.UpdateUserDisplayName(stringId, updateDTO.FirstName.Trim() + " " + updateDTO.LastName.Trim());
            }

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
        public async Task<IActionResult> UpdateUserDetailsAsync([Required]string id, [Required]UserUpdateDTO updateDTO)
        {
            var user = await _userService.UpdateUserDetailsAsync(id, updateDTO);

            var check = await _firestoreProvider.GetUser(id);
            if (check)
            {
                await _firestoreProvider.UpdateUserDisplayName(id, updateDTO.FirstName.Trim() + " " + updateDTO.LastName.Trim());
            }

            return ResponseFactory.Ok(user);
        }

        ///<summary>
        /// Update user avatar (this function will also add avatar if user currently don't have one)
        /// </summary>
        /// <remarks>Update user's avatar</remarks>
        /// <param name="avatar"></param>
        /// <returns></returns>
        [HttpPost("media")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserMediaReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateUserAvatarAsync([Required]IFormFile avatar)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _userMediaService.UploadUserAvatar(avatar, stringId);

            var check = await _firestoreProvider.GetUser(stringId);
            if (check)
            {
                await _firestoreProvider.UpdateUserPhotoUrl(stringId, result.Media.Url);
            }

            return ResponseFactory.Ok(result);
        }

        ///<summary>
        /// Update user credentails (this function will also add credentials if user currently don't have one)
        /// </summary>
        /// <remarks>Update user's credentials</remarks>
        /// <param name="userMediaCredentialsWriteDTO"></param>
        /// <returns></returns>
        [HttpPost("media-credentials")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ICollection<UserMediaReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateUserCredentialImagesAsync([FromForm]UserMediaCredentialsWriteDTO userMediaCredentialsWriteDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _userMediaService.UploadUserCredentialForVerification(stringId, 
                userMediaCredentialsWriteDTO.SchoolId, 
                userMediaCredentialsWriteDTO.CCIDFront, 
                userMediaCredentialsWriteDTO.CCIDBack,
                userMediaCredentialsWriteDTO.StudentCard);
            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Change user's verify status
        /// </summary>
        /// <remarks>Change user's verify status</remarks>
        /// <returns></returns>
        [HttpPatch("update-verify-status")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<UserDetailsReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ChangeUserVerifyStatus([Required]UserVerifyStatusUpdateDTO updateDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            string[] roles = { "Manager", "Storage Manager", "Admin" };
            await _firebaseAuthService.CheckUserRoles(stringId, roles);
            var result = await _userService.ChangeUserVerifyStatusAsync(updateDTO);

            //Noti
            //if(updateDTO.VerifyStatus == Core.Enums.UserVerifyStatus.VERIFIED)
            if (result.VerifyStatus == Core.Enums.UserVerifyStatus.VERIFIED)
            {
                await NotificationExtensions
                .Notify(_userDeviceService, _notificationService, updateDTO.UserId, "Your Account Status has been Verified!",
                "Your Account Status has been Verified!", Core.Enums.NotificationType.UserVerifyStatus);
            }
            if (result.VerifyStatus == Core.Enums.UserVerifyStatus.NOT_VERIFIED)
            {
                await NotificationExtensions
                .Notify(_userDeviceService, _notificationService, updateDTO.UserId, "Your Account Verification Request has been Rejected!",
                "Your Account Verification Request has been Rejected! Please update your credentials again!", Core.Enums.NotificationType.UserVerifyStatus);
            }
            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Generate QR Code
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("qrcode")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GenerateQRCode(string userId)
        {
            var result = await QRCodeExtensions.GenerateQRCode(userId);

            return File(result, "image/bmp");
        }

        /// <summary>
        /// Get all Storage Managers no paginated (only Active)
        /// </summary>
        /// <returns></returns>
        [HttpGet("list-all-storage-managers")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserDetailsReadDTO[]>))]
        public async Task<IActionResult> ListAllStorageManagers()
        {
            var result = await _userService.ListAllStorageManagersAsync();

            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get all Storage Managers no paginated (only Active)
        /// </summary>
        /// <param name="campusId"></param>
        /// <returns></returns>
        [HttpGet("list-all-storage-managers-by-campus")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserDetailsReadDTO[]>))]
        public async Task<IActionResult> ListAllStorageManagersByCampusId(int campusId)
        {
            var result = await _userService.ListAllStorageManagersByCampusIdAsync(campusId);

            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get current logged in user
        /// </summary>
        /// <returns></returns>
        [HttpGet("current-user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserDetailsReadDTO[]>))]
        public async Task<IActionResult> GetCurrentUser()
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _userService.GetUserAsync(stringId);

            return ResponseFactory.Ok(result);
        }
    }
}