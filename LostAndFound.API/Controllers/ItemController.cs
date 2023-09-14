using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.ItemMedia;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Implementations;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LostAndFound.API.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IItemMediaService _itemMediaService;

        public ItemController(IItemService itemService, IItemMediaService itemMediaService)
        {
            _itemService = itemService;
            _itemMediaService = itemMediaService;
        }

        /// <summary>
        /// Query Items with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [QueryResponseCache(typeof(ItemQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<ItemReadDTO>>))]
        public async Task<IActionResult> Query([FromQuery] ItemQuery query)
        {
            var paginatedItemDTO = await _itemService.QueryItemAsync(query);

            return ResponseFactory.PaginatedOk(paginatedItemDTO);
        }

        /// <summary>
        /// Query Items ignoring IsActive status with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [QueryResponseCache(typeof(ItemQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<ItemReadDTO>>))]
        public async Task<IActionResult> QueryIgnoreStatus([FromQuery] ItemQuery query)
        {
            var paginatedItemDTO = await _itemService.QueryItemIgnoreStatusAsync(query);

            return ResponseFactory.PaginatedOk(paginatedItemDTO);
        }

        /// <summary>
        /// Get Item By Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id/{itemId}")]
        [QueryResponseCache(typeof(ItemQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<MediaReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetItemByID([Required] int itemId)
        {
            var item = await _itemService.FindItemByIdAsync(itemId);

            return ResponseFactory.Ok(item);
        }

        /// <summary>
        /// Find Item by name
        /// </summary>
        /// <returns></returns>
        [HttpGet("name/{name}")]
        [QueryResponseCache(typeof(ItemQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<MediaReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetItemByName([Required] string name)
        {
            var item = await _itemService.FindItemNameAsync(name);

            return ResponseFactory.Ok(item);
        }

        /// <summary>
        /// Update Item status
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpPatch("change-status/{itemId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateItemStatus([Required] int itemId)
        {
            await _itemService.UpdateItemStatusAsync(itemId);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Soft delete an item
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{itemId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteItem([Required] int itemId)
        {
            await _itemService.DeleteItemAsync(itemId);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Get all Item's Medias
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("{itemId}/media")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemMediaReadDTO[]>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAllItemMedia([Required] int itemId)
        {
            return ResponseFactory.Ok(await _itemMediaService.GetItemMedias(itemId));
        }

        /// <summary>
        /// Upload Item's media files
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("{itemId}/media")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemMediaReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> CreatePostMedias([Required] int itemId, [Required] IFormFile[] files)
        {
            var itemMedias = await _itemMediaService.UploadItemMedias(User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value, itemId, files);
            return ResponseFactory.CreatedAt(nameof(GetAllItemMedia),
                                            nameof(ItemController),
                                            new { itemId = itemId },
                                            itemMedias);
        }

        /// <summary>
        /// Delete Item's media file (soft)
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        [HttpDelete("{itemId}/media")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteMedia([Required] int itemId, [Required] Guid mediaId)
        {
            await _itemMediaService.DeleteItemMedia(User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value, itemId, mediaId);
            return ResponseFactory.NoContent();
        }
    }
}
