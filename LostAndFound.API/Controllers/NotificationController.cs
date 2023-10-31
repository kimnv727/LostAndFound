using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.API.Extensions;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Notification;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.API.Controllers
{
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IUserDeviceService _userDeviceService;

        public NotificationController(INotificationService notificationService, IUserService userService, IUserDeviceService userDeviceService)
        {
            _notificationService = notificationService;
            _userService = userService;
            _userDeviceService = userDeviceService;
        }
        
        /// <summary>
        /// Get notification's details by id
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        [HttpGet("{notificationId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<NotificationReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetNotification(int notificationId)
        {
            var notification = await _notificationService.GetNotification(notificationId);

            return ResponseFactory.Ok(notification);
        }
        
        ///<summary>
        /// Create new notification
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<NotificationReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateNotification(NotificationWriteDTO writeDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _notificationService.CreateNotification(writeDTO, stringId);

            return ResponseFactory.CreatedAt(
                (nameof(GetNotification)), 
                nameof(NotificationController), 
                new { notificationId = result.Id }, 
                result);
        }
        
        /// <summary>
        /// Get total number of Unread Notification by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("count-unread-notification/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> CountUnreadNotificationOfAUser(string userId)
        {
            return ResponseFactory.Ok(await _notificationService.CountUnreadOfUser(userId));
        }
        
        /// <summary>
        /// Get all Unread Notification by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("get-unread-notification/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<NotificationReadDTO>))]
        public async Task<IActionResult> GetAllUnreadNotificationsByUserId([Required] string userId)
        {
            var result = await _notificationService.GetUnreadNotificationsOfUser(userId);

            return ResponseFactory.PaginatedOk(result);
        }
        
        /// <summary>
        /// Get all  Notification by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("get-all-notification/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<NotificationReadDTO>))]
        public async Task<IActionResult> GetAllNotificationsByUserId([Required] string userId)
        {
            var result = await _notificationService.GetAllNotificationsOfUser(userId);

            return ResponseFactory.PaginatedOk(result);
        }

        /// <summary>
        /// Mark as read
        /// </summary>
        /// <remarks></remarks>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        [HttpPatch("mark-as-read/{notificationId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _notificationService.MarkAsRead(notificationId);

            return ResponseFactory.NoContent();
        }
        
        /// <summary>
        /// Mark all as read of a User
        /// </summary>
        /// <remarks></remarks>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPatch("mark-all-read/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> MarkAllAsRead(string userId)
        {
            await _notificationService.MarkAllAsRead(userId);

            return ResponseFactory.NoContent();
        }
        
        ///<summary>
        /// Push notification
        /// </summary>
        /// <param name="pushNotification"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("push")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<ActionResult> PushNotification(PushNotification notification)
        {
            //string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            if (!string.IsNullOrWhiteSpace(notification.UserId))
            {
                var user = await _userService.GetUserAsync(notification.UserId);
                if (user != null)
                {
                   switch (notification.NotificationType)
                    {
                        case NotificationType.Chat:
                            await NotificationExtensions
                                    .NotifyChatToUser(_userDeviceService, _notificationService,notification.UserId, notification.Title, notification.Content);
                                return Ok();
                        case NotificationType.OwnItemClaim:
                            await NotificationExtensions
                                .NotifyChatToUser(_userDeviceService, _notificationService,notification.UserId, notification.Title, notification.Content);
                            return Ok();
                        case NotificationType.PostGotReplied:
                            await NotificationExtensions
                                .NotifyChatToUser(_userDeviceService, _notificationService,notification.UserId, notification.Title, notification.Content);
                            return Ok();
                        case NotificationType.CommentGotReplied:
                            await NotificationExtensions
                                .NotifyChatToUser(_userDeviceService, _notificationService,notification.UserId, notification.Title, notification.Content);
                            return Ok();
                        case NotificationType.GiveawayResult:
                            await NotificationExtensions
                                .NotifyChatToUser(_userDeviceService, _notificationService,notification.UserId, notification.Title, notification.Content);
                            return Ok();
                        default:
                        return BadRequest();
                    }
                }
            }
            return Forbid();
        }
    }
}