using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Category;

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

        
    }
}
