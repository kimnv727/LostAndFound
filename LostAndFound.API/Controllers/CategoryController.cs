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
        /// Query Categories with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [QueryResponseCache(typeof(CategoryQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<CategoryQuery>>))]
        public async Task<IActionResult> Query([FromQuery] CategoryQuery query)
        {
            var paginatedCategoryDTO = await _categoryService.QueryCategoryAsync(query);

            return ResponseFactory.PaginatedOk(paginatedCategoryDTO);
        }

        /// <summary>
        /// Get Category By Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id/{CategoryId}")]
        [QueryResponseCache(typeof(CategoryQuery))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FindCategoryById([Required] int CategoryId)
        {
            var category = await _categoryService.FindCategoryByIdAsync(CategoryId);

            return ResponseFactory.Ok(category);
        }

        /// <summary>
        /// Get Category by name
        /// </summary>
        /// <returns></returns>
        [HttpGet("name/{name}")]
        [QueryResponseCache(typeof(CategoryQuery))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FindCategoryByName([Required] string name)
        {
            var category = await _categoryService.FindCategoryByNameAsync(name);

            return ResponseFactory.Ok(category);
        }
        
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
    }
}
