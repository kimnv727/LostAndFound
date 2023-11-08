using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.Location;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.API.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        
        /// <summary>
        /// Query locations with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet("paginated")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<LocationReadDTO>>))]
        public async Task<IActionResult> PaginatedQuery([FromQuery] LocationQuery query)
        {
            var paginatedLocationDTO = await _locationService.QueryLocationWithPaginationAsync(query);

            return ResponseFactory.PaginatedOk(paginatedLocationDTO);
        }

        /// <summary>
        /// Query locations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<LocationReadDTO>>))]
        public async Task<IActionResult> Query([FromQuery] LocationQuery query)
        {
            var locationDTO = await _locationService.QueryLocationAsync(query);

            return ResponseFactory.Ok(locationDTO);
        }

        /// <summary>
        /// List locations
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<LocationReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ListAll()
        {
            var locations = await _locationService.ListAllAsync();

            return ResponseFactory.Ok(locations);
        }

        /// <summary>
        /// Get Location By Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id/{LocationId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LocationReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetLocationByID([Required] int LocationId)
        {
            var location = await _locationService.FindLocationByIdAsync(LocationId);

            return ResponseFactory.Ok(location);
        }
        
        /// <summary>
        /// Get Location by name
        /// </summary>
        /// <returns></returns>
        [HttpGet("name/{locationName}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetLocationByName([Required] string locationName)
        {
            var item = await _locationService.FindLocationByNameAsync(locationName);

            return ResponseFactory.Ok(item);
        }
        
        ///<summary>
        /// Update location information
        /// </summary>
        /// <remarks>Update Location's information</remarks>
        /// <returns></returns>
        [HttpPut("{LocationId:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateUserDetailsAsync(int LocationId, LocationWriteDTO writeDTO)
        {
            var location = await _locationService.UpdateLocationDetailsAsync(LocationId, writeDTO);

            return ResponseFactory.Ok(location);
        }
        
        ///<summary>
        /// Create new location
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<LocationReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreatePost(LocationWriteDTO writeDTO)
        {
            var result = await _locationService.CreateItemAsync(writeDTO);

            return ResponseFactory.Ok(result);
        }
        
        /// <summary>
        /// Soft delete a location
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{LocationId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteLocation([Required] int LocationId)
        {
            await _locationService.DeleteLocationAsync(LocationId);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Change location's IsActive value
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<LocationReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ChangeLocationIsActiveStatus([Required] int id)
        {
            return ResponseFactory.Ok(await _locationService.ChangeLocationStatusAsync(id));
        }
    }
}