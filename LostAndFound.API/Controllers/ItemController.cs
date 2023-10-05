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
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.ItemFlag;

namespace LostAndFound.API.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IItemMediaService _itemMediaService;
        private readonly IItemFlagService _itemFlagService;
        private readonly IItemBookmarkService _itemBookmarkService;
        private readonly ICategoryRepository _categoryRepository;

        public ItemController(IItemService itemService, IItemMediaService itemMediaService, IItemFlagService itemFlagService, IItemBookmarkService itemBookmarkService, ICategoryRepository categoryRepository)
        {
            _itemService = itemService;
            _itemMediaService = itemMediaService;
            _itemFlagService = itemFlagService;
            _itemBookmarkService = itemBookmarkService;
            _categoryRepository = categoryRepository;
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
        /// Query Items by Item status with pagination
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
        /// Find Item By Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id/{itemId}")]
        //[QueryResponseCache(typeof(ItemQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemDetailReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FindItemById([Required] int itemId)
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
        public async Task<IActionResult> FindItemByName([Required] string name)
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
            //TODO: Change isActive to ItemStatus
            await _itemService.UpdateItemStatusAsync(itemId);
            return ResponseFactory.NoContent();
        }
        
        ///<summary>
        /// Update item information
        /// </summary>
        /// <remarks>Update Item's information</remarks>
        /// <returns></returns>
        [HttpPut("{itemId:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateItemDetailsAsync(int itemId, ItemUpdateDTO updateDTO)
        {
            
            var item = await _itemService.UpdateItemDetailsAsync(itemId, updateDTO);

            return ResponseFactory.Ok(item);
        }
 
        ///<summary>
        /// Create new item
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<ItemReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateItem([FromForm]ItemWriteDTO writeDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _itemService.CreateItemAsync(stringId, writeDTO);

            return ResponseFactory.Ok(result);
        }
        
        /// <summary>
        /// Delete an item
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
        
        
        
        /// <summary>
        /// Count total flags of an item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("count-item-flag/{itemId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> CountItemFlagOfAnItem(int itemId)
        {
            return ResponseFactory.Ok(await _itemFlagService.CountItemFlagAsync(itemId));
        }
        
        /// <summary>
        /// Get item flag details
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("get-item-flag")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemFlagReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetItemFlag(string userId, int itemId)
        {
            var itemFlag = await _itemFlagService.GetItemFlag(userId, itemId);

            return ResponseFactory.Ok(itemFlag);
        }
        
        /// <summary>
        /// Get all own ItemFlags
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-own-item-flag")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemReadDTO[]>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAllOwnItemFlag()
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            return ResponseFactory.Ok(await _itemFlagService.GetOwnItemFlags(stringId));
        }
        
        /// <summary>
        /// Flag an item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpPost("flag-an-item/{itemId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemFlagReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FlagAnItem(int itemId, ItemFlagReason reason)
        {
            if (!Enum.IsDefined(reason))
            {
                throw new Exception();
            }
            
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var itemFlag = await _itemFlagService.FlagAnItem(stringId, itemId, reason);
            
            return ResponseFactory.CreatedAt(nameof(GetItemFlag), 
                nameof(ItemController), 
                new { userId = itemFlag.UserId, itemId = itemFlag.ItemId }, 
                itemFlag);
        }
        
        
        
        /// <summary>
        /// Count total bookmarks of an item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("count-item-bookmark/{itemId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> CountBookmarkOfAnItem(int itemId)
        {
            return ResponseFactory.Ok(await _itemBookmarkService.CountItemBookmarkAsync(itemId));
        }
        
        /// <summary>
        /// Get item bookmark details
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("get-item-bookmark")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemFlagReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetItemBookmark(string userId, int itemId)
        {
            var itemFlag = await _itemBookmarkService.GetItemBookmark(userId, itemId);

            return ResponseFactory.Ok(itemFlag);
        }
        
        /// <summary>
        /// Get all own bookmarks
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-own-item-bookmark")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemReadDTO[]>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAllOwnItemBookmark()
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            return ResponseFactory.Ok(await _itemBookmarkService.GetOwnItemBookmarks(stringId));
        }
        
        /// <summary>
        /// Bookmark an item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpPost("bookmark-an-item/{itemId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemFlagReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> BookmarkAnItem(int itemId)
        {

            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var itemFlag = await _itemBookmarkService.BookmarkAnItem(stringId, itemId);
            
            return ResponseFactory.CreatedAt(nameof(GetItemBookmark), 
                nameof(ItemController), 
                new { userId = itemFlag.UserId, itemId = itemFlag.ItemId }, 
                itemFlag);
        }
    }
}
