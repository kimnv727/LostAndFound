using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostAndFound.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : Controller
    {
        //private readonly ICategoryService _categoryService;

        /*public CategoryController(ICategoryService categoryService)
        {
            //_categoryService = categoryService
        }*/

        /// <summary>
        /// Query Categories with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [QueryResponseCache(typeof(ItemQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<ItemReadDTO>>))]
        public async Task<IActionResult> Query([FromQuery] ItemQuery query)
        {
            return null;
        }

        
    }
}
