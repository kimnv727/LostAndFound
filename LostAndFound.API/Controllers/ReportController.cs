using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.DTOs.ViolationReport;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LostAndFound.API.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _violationReportService;

        public ReportController(
            IReportService violationReportService)
        {
            _violationReportService = violationReportService;
        }

        ///<summary>
        /// Create new violation report
        /// </summary>
        /// <param name="createDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<ReportReadDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportDTO createDTO)
        {
            var result = await _violationReportService.CreateReportAsync(createDTO, 
                User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            return ResponseFactory.CreatedAt(nameof(GetReport)
                , nameof(ReportController), new { id = result.Id }
                , result);
        }

        ///<summary>
        /// Get all violation reports
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<ReportReadDTO>))]
        public async Task<IActionResult> GetReportList
            ([FromQuery] ReportQuery query)
        {
            var result = await _violationReportService.QueryViolationReport(query);

            return ResponseFactory.PaginatedOk(result);
        }

        ///<summary>
        /// Get violation report's detail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ReportReadDTO>))]
        public async Task<IActionResult> GetReport(int id)
        {
            var result = await _violationReportService.GetReportById(id);

            return ResponseFactory.PaginatedOk(result);
        }
    }
}
