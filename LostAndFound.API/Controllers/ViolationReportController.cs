using LostAndFound.API.ResponseWrapper;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.ViolationReport;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LostAndFound.API.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ViolationReportController : ControllerBase
    {
        private readonly IViolationReportService _violationReportService;

        public ViolationReportController(
            IViolationReportService violationReportService)
        {
            _violationReportService = violationReportService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportDTO report)
        {
            await _violationReportService.CreateReportAsync(report, 
                User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            return ResponseFactory.Ok("Create report success");
        }
    }
}
