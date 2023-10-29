using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.CategoryGroup;
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
    [Route("api/categoryGroups")]
    [ApiController]
    public class CategoryGroupController : Controller
    {
        private readonly ICategoryGroupService _categoryGroupService;

        public CategoryGroupController(ICategoryGroupService categoryGroupService)
        {
            _categoryGroupService = categoryGroupService;
        }

        /// <summary>
        /// Query category group with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [QueryResponseCache(typeof(CategoryGroupQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<CategoryGroupReadDTO>>))]
        public async Task<IActionResult> Query([FromQuery] CategoryGroupQuery query)
        {
            var paginatedCategoryGroupsDTO = await _categoryGroupService.QueryCategoryGroupAsync(query);
            return ResponseFactory.PaginatedOk(paginatedCategoryGroupsDTO);
        }

        /// <summary>
        /// List all category groups
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<CategoryGroupReadDTO>>))]
        public async Task<IActionResult> ListAll()
        {
            var categoryGroups = await _categoryGroupService.ListAllAsync();
            return Ok(categoryGroups);
        }

        /// <summary>
        /// Find category group by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id/{categoryGroupId}")]
        [QueryResponseCache(typeof(CategoryGroupQuery))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FindCategoryGroupByID([Required] int categoryGroupId)
        {
            var CategoryGroup = await _categoryGroupService.GetCategoryGroupByIdAsync(categoryGroupId);
            return ResponseFactory.PaginatedOk(CategoryGroup);
        }

        ///<summary>
        /// Update categoryGroup's information
        /// </summary>
        /// <returns></returns>
        [HttpPut("{categoryGroupId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateCategoryGroupDetailsAsync(int categoryGroupId, CategoryGroupWriteDTO categoryGroupWriteDTO)
        {
            var categoryGroup = await _categoryGroupService.UpdateCategoryGroupDetailsAsync(categoryGroupId, categoryGroupWriteDTO);
            return ResponseFactory.PaginatedOk(categoryGroup);
        }
        
        ///<summary>
        /// Create new categoryGroup
        /// </summary>
        /// <param name="categoryGroupWriteDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<CategoryGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateCategoryGroup(CategoryGroupWriteDTO categoryGroupWriteDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _categoryGroupService.CreateCategoryGroupAsync(stringId, categoryGroupWriteDTO);

            return ResponseFactory.PaginatedOk(result);
        }
        
        /// <summary>
        /// Delete a category group
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{categoryGroupId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteCategoryGroup([Required] int categoryGroupId)
        {
            await _categoryGroupService.DeleteCategoryGroupAsync(categoryGroupId);
            return ResponseFactory.NoContent();
        }
        
    }
}