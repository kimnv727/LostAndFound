using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Category;
using Microsoft.AspNetCore.Authorization;

namespace LostAndFound.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Query categories with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<CategoryQuery>>))]
        public async Task<IActionResult> Query([FromQuery] CategoryQuery query)
        {
            var paginatedCategoryDTO = await _categoryService.QueryCategoryAsync(query);

            return ResponseFactory.Ok<Infrastructure.DTOs.Common.PaginatedResponse<CategoryReadDTO>>(paginatedCategoryDTO);
        }

        /// <summary>
        /// List all categories
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAll()
        {
            var categoryDTO = await _categoryService.ListAllAsync();

            return Ok(categoryDTO);
        }

        /// <summary>
        /// Get Category By Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FindCategoryById([Required] int categoryId)
        {
            var category = await _categoryService.FindCategoryByIdAsync(categoryId);

            return ResponseFactory.Ok(category);
        }

        /*/// <summary>
        /// Get Category by name
        /// </summary>
        /// <returns></returns>
        [HttpGet("name/{categoryName}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FindCategoryByName([Required] string categoryName)
        {
            var category = await _categoryService.FindCategoryByNameAsync(categoryName);

            return ResponseFactory.Ok(category);
        }*/
        
        ///<summary>
        /// Create new category
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<CategoryReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreatePost( CategoryWriteDTO writeDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _categoryService.CreateCategoryAsync(stringId, writeDTO);
            return ResponseFactory.Ok(result);
        }
        
        ///<summary>
        /// Update category information
        /// </summary>
        /// <remarks>Update category's information</remarks>
        /// <returns></returns>
        [HttpPut("{categoryId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateCategoryDetailsAsync(int categoryId, CategoryWriteDTO writeDTO)
        {
            
            var category = await _categoryService.UpdateCategoryAsync(categoryId, writeDTO);
            return ResponseFactory.Ok(category);
        }
        
        /// <summary>
        /// Delete a category
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteItem([Required] int categoryId)
        {
            await _categoryService.DeleteCategoryAsync(categoryId);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Change category's IsActive value
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<CategoryReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ChangeCategoryIsActiveStatus([Required] int id)
        {
            return ResponseFactory.Ok(await _categoryService.ChangeCategoryStatusAsync(id));
        }
    }
}
