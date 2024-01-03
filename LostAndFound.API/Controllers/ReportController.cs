using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.DTOs.Report;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.API.Extensions;
using LostAndFound.Core.Enums;

namespace LostAndFound.API.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IUserDeviceService _userDeviceService;

        public ReportController(IReportService reportService, INotificationService notificationService, 
            IUserService userService, IUserDeviceService userDeviceService)
        {
            _reportService = reportService;
            _notificationService = notificationService;
            _userService = userService;
            _userDeviceService = userDeviceService;
        }

        ///<summary>
        /// Create new report
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<ReportReadDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateReport([FromForm] ReportWriteDTO writeDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _reportService.CreateReportAsync(stringId, writeDTO);

            return ResponseFactory.CreatedAt(nameof(GetReport)
                , nameof(ReportController), new { reportId = result.Id }
                , result);
        }

        ///<summary>
        /// Get all reports
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<ReportReadDTO>))]
        public async Task<IActionResult> GetReportList([FromQuery] ReportQuery query)
        {
            var result = await _reportService.QueryReports(query);

            return ResponseFactory.PaginatedOk(result);
        }

        ///<summary>
        /// Get report's detail by id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [HttpGet("{reportId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ReportReadDTO>))]
        public async Task<IActionResult> GetReport(int reportId)
        {
            var result = await _reportService.GetReportById(reportId);

            return ResponseFactory.Ok(result);
        }

        ///<summary>
        /// Get report's detail by user and item Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("get-by-user-and-item")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ReportReadDTO>))]
        public async Task<IActionResult> GetReportByUserAndItemId(string userId, int itemId)
        {
            var result = await _reportService.GetReportByUserAndItemId(userId, itemId);

            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get all reports by userId (only Active)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("get-by-user/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ReportReadDTO[]>))]
        public async Task<IActionResult> GetAllReportsByUserId(string userId)
        {
            var result = await _reportService.GetReportByUserId(userId);

            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get all posts by userId (only Active)
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("get-by-item/{itemId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ReportReadDTO[]>))]
        public async Task<IActionResult> GetAllPostsByItemId(int itemId)
        {
            var result = await _reportService.GetReportByItemId(itemId);

            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Update Post Status
        /// </summary>
        /// <remarks></remarks>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [HttpPatch("status/{reportId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ReportReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateReportStatus(int reportId, ReportStatusUpdateDTO updateDTO)
        {
            var result = await _reportService.UpdateReportStatusAsync(reportId, updateDTO);

            //Noti
            if (updateDTO.ReportStatus == Core.Enums.ReportStatus.RESOLVED)
            {
                await NotificationExtensions
                .Notify(_userDeviceService, _notificationService, result.UserId, "Your Report about Item with ID " + result.ItemId + " has been Resolved!",
                "Your Report about Item with ID " + result.ItemId + " has been Resolved! Please wait for our Email for more Details!", NotificationType.Report);
            }

            return ResponseFactory.Ok(result);
        }
    }
}
