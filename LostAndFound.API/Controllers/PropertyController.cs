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
    [Route("api/properties")]
    [ApiController]
    public class PropertyController : Controller
    {
        private readonly ICategoryService _categoryService;

        public PropertyController(ICategoryService categoryService)
        {
            //_categoryService = categoryService
        }

        /// <summary>
        /// Query Properties
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