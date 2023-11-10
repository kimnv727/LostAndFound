using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.CategoryGroup;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<CategoryGroupReadDTO>>))]
        public async Task<IActionResult> ListAll()
        {
            var categoryGroups = await _categoryGroupService.ListAllWithCategoriesAsync();
            return ResponseFactory.Ok(categoryGroups);
        }

        /// <summary>
        /// Find category group by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id/{categoryGroupId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FindCategoryGroupByID([Required] int categoryGroupId)
        {
            var CategoryGroup = await _categoryGroupService.GetCategoryGroupByIdAsync(categoryGroupId);
            return ResponseFactory.Ok(CategoryGroup);
        }

        ///<summary>
        /// Update categoryGroup's information
        /// </summary>
        /// <returns></returns>
        [HttpPut("{categoryGroupId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateCategoryGroupDetailsAsync(int categoryGroupId, CategoryGroupWriteDTO categoryGroupWriteDTO)
        {
            var categoryGroup = await _categoryGroupService.UpdateCategoryGroupDetailsAsync(categoryGroupId, categoryGroupWriteDTO);
            return ResponseFactory.Ok(categoryGroup);
        }
        
        ///<summary>
        /// Create new Category Group
        /// </summary>
        /// <param name="categoryGroupWriteDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<CategoryGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateCategoryGroup(CategoryGroupWriteDTO categoryGroupWriteDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _categoryGroupService.CreateCategoryGroupAsync(stringId, categoryGroupWriteDTO);

            return ResponseFactory.Ok(result);
        }
        
        /// <summary>
        /// Delete a category group
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{categoryGroupId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteCategoryGroup([Required] int categoryGroupId)
        {
            await _categoryGroupService.DeleteCategoryGroupAsync(categoryGroupId);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Change Category Group's IsActive value
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<CategoryGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ChangeCategoryIsActiveStatus([Required] int id)
        {
            return ResponseFactory.Ok(await _categoryGroupService.ChangeCategoryGroupStatusAsync(id));
        }

    }
}