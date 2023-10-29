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
using AutoMapper;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Property;

namespace LostAndFound.API.Controllers
{
    [Route("api/properties")]
    [ApiController]
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        
        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        /// <summary>
        /// Query properties with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [QueryResponseCache(typeof(PropertyQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<PropertyReadDTO>>))]
        public async Task<IActionResult> Query([FromQuery] PropertyQuery query)
        {
            var paginatedPropertiesDTO = await _propertyService.QueryPropertyAsync(query);
            return ResponseFactory.PaginatedOk(paginatedPropertiesDTO);
        }

        /// <summary>
        /// List all properties
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<PropertyReadDTO>>))]
        public async Task<IActionResult> ListAll()
        {
            var propertiesDTO = await _propertyService.ListAllAsync();
            return Ok(propertiesDTO);
        }

        /// <summary>
        /// Find property by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id/{propertyId}")]
        [QueryResponseCache(typeof(PropertyQuery))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FindPropertyByID([Required] int propertyId)
        {
            var Property = await _propertyService.GetPropertyByIdAsync(propertyId);
            return ResponseFactory.PaginatedOk(Property);
        }
        
        /// <summary>
        /// Change Property status
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        [HttpPatch("change-status/{propertyId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ChangePropertyStatus([Required] int propertyId)
        {
            await _propertyService.ChangePropertyStatusAsync(propertyId);
            return ResponseFactory.NoContent();
        }
        
        ///<summary>
        /// Update property's information
        /// </summary>
        /// <returns></returns>
        [HttpPut("{propertyId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdatePropertyDetailsAsync(int propertyId, PropertyWriteDTO propertyWriteDTO)
        {
            var property = await _propertyService.UpdatePropertyDetailsAsync(propertyId, propertyWriteDTO);
            return ResponseFactory.PaginatedOk(property);
        }
        
        ///<summary>
        /// Create new property
        /// </summary>
        /// <param name="propertyWriteDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<PropertyReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateProperty(PropertyWriteDTO propertyWriteDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _propertyService.CreatePropertyAsync(stringId, propertyWriteDTO);

            return ResponseFactory.PaginatedOk(result);
        }
        
        /// <summary>
        /// Delete an property
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{propertyId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteProperty([Required] int propertyId)
        {
            await _propertyService.DeletePropertyAsync(propertyId);
            return ResponseFactory.NoContent();
        }
        
    }
}