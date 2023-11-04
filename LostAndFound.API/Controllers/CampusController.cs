using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.Property;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LostAndFound.API.Controllers
{
    [Route("api/campuses")]
    [ApiController]
    public class CampusController : Controller
    {
        private readonly ICampusService _CampusService;
        
        public CampusController(ICampusService CampusService)
        {
            _CampusService = CampusService;
        }

        /// <summary>
        /// Query Campuses with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<CampusReadDTO>>))]
        public async Task<IActionResult> Query([FromQuery] CampusQuery query)
        {
            var paginatedCampusDTO = await _CampusService.QueryCampusAsync(query);
            return ResponseFactory.PaginatedOk(paginatedCampusDTO);
        }

        /// <summary>
        /// List all Campuses
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<CampusReadDTO>>))]
        public async Task<IActionResult> ListAll()
        {
            var campusDTO = await _CampusService.ListAllAsync();
            return Ok(campusDTO);
        }

        /// <summary>
        /// Find campus by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id/{CampusId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FindCampusByID([Required] int CampusId)
        {
            var campus = await _CampusService.GetCampusByIdAsync(CampusId);
            return ResponseFactory.Ok(campus);
        }
        
        /// <summary>
        /// Change campus status
        /// </summary>
        /// <param name="CampusId"></param>
        /// <returns></returns>
        [HttpPatch("change-status/{CampusId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ChangeCampusStatus([Required] int CampusId)
        {
            return ResponseFactory.Ok(await _CampusService.ChangeCampusStatusAsync(CampusId));
        }
        
        ///<summary>
        /// Update Campus's information
        /// </summary>
        /// <returns></returns>
        [HttpPut("{CampusId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateCampusDetailsAsync(int CampusId, CampusWriteDTO CampusWriteDTO)
        {
            var campus = await _CampusService.UpdateCampusDetailsAsync(CampusId, CampusWriteDTO);
            return ResponseFactory.Ok(campus);
        }
        
        ///<summary>
        /// Create new Campus
        /// </summary>
        /// <param name="CampusWriteDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<CampusReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateCampus(CampusWriteDTO CampusWriteDTO)
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _CampusService.CreateCampusAsync(userId, CampusWriteDTO);

            return ResponseFactory.Ok(result);
        }
        
        /// <summary>
        /// Delete a Campus
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{CampusId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteCampus([Required] int CampusId)
        {
            await _CampusService.DeleteCampusAsync(CampusId);
            return ResponseFactory.NoContent();
        }
        
    }
}