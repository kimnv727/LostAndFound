﻿
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
using System.Xml.Linq;
using LostAndFound.Infrastructure.DTOs.ItemClaim;
using Microsoft.VisualBasic;
using LostAndFound.API.Authentication;
using System.Data;
using Firebase.Auth;
using LostAndFound.Core.Exceptions.ItemClaim;

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
        private readonly IItemClaimService _itemClaimService;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly ICabinetService _cabinetService;

        public ItemController(IItemService itemService, IItemMediaService itemMediaService, IItemFlagService itemFlagService, IItemBookmarkService itemBookmarkService,
            ICategoryRepository categoryRepository, IItemClaimService itemClaimService, IFirebaseAuthService firebaseAuthService, ICabinetService cabinetService)
        {
            _itemService = itemService;
            _itemMediaService = itemMediaService;
            _itemFlagService = itemFlagService;
            _itemBookmarkService = itemBookmarkService;
            _categoryRepository = categoryRepository;
            _itemClaimService = itemClaimService;
            _firebaseAuthService = firebaseAuthService;
            _cabinetService = cabinetService;
        }

        /// <summary>
        /// Query Items with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<ItemReadDTO>>))]
        public async Task<IActionResult> Query([FromQuery]ItemQueryWithStatus query)
        {
            var paginatedItemDTO = await _itemService.QueryItemAsync(query);

            return ResponseFactory.PaginatedOk(paginatedItemDTO);
        }

        /// <summary>
        /// Find Item By Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id/{itemId}")]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<MediaReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FindItemByName([Required] string name)
        {
            var item = await _itemService.FindItemByNameAsync(name);
            return ResponseFactory.Ok(item);
        }

        /// <summary>
        /// Update Item status
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemStatus"></param>
        /// <remarks>Update Item's status</remarks>
        /// <returns></returns>
        [HttpPatch("change-status/{itemId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateItemStatus([Required] int itemId, ItemStatus itemStatus)
        {
            await _itemService.ChangeItemStatusAsync(itemId, itemStatus);
            return NoContent();
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
        public async Task<IActionResult> CreateItem([FromForm] ItemWriteDTO writeDTO)
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
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            return ResponseFactory.Ok(await _itemBookmarkService.GetOwnItemBookmarks(userId));
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

            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var itemFlag = await _itemBookmarkService.BookmarkAnItem(userId, itemId);

            return ResponseFactory.CreatedAt(nameof(GetItemBookmark),
                nameof(ItemController),
                new { userId = itemFlag.UserId, itemId = itemFlag.ItemId },
                itemFlag);
        }

        /// <summary>
        /// Claim an item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpPost("claim")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemClaimReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ClaimAnItem([Required]int itemId)
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            await _itemClaimService.ClaimAnItemAsync(itemId, userId);

            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Unclaim an item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpPost("unclaim")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemClaimReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UnClaimAnItem([Required]int itemId)
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            await _itemClaimService.UnclaimAnItemAsync(itemId, userId);

            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Accept a claim (Will disable any other claims)
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("accept")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> AcceptAClaimAsync([Required]int itemId, [Required]string userId)
        {
            string currentUserId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            if (await _itemService.CheckItemFounderAsync(itemId, currentUserId))
            {
                await _itemService.AcceptAClaimAsync(itemId, userId);
            }
            else throw new ItemFounderNotMatchException();

            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Deny a claim
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("deny")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DenyAClaimAsync([Required] int itemId, [Required] string userId)
        {
            string currentUserId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            if (await _itemService.CheckItemFounderAsync(itemId, currentUserId))
            {
                await _itemService.DenyAClaimAsync(itemId, userId);
            }
            else throw new ItemFounderNotMatchException();
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// (For item founder) Get an item and (all of) its ItemClaims, by itemId
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("claims/founder/item/{itemId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemClaimReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAllItemsWithClaimsForCreator(int itemId)
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            return ResponseFactory.Ok(await _itemService.GetAnItemWithClaimsForFounder(userId, itemId));
        }

        /// <summary>
        /// (For member) Get an item and (only this user's) ItemClaims, by itemId
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("claims/member/item/{itemId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemClaimReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetThisMemberClaimByItemId(int itemId)
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            return ResponseFactory.Ok(await _itemService.GetAnItemWithClaimsForMember(userId, itemId));
        }

        /// <summary>
        /// (Manager) Get an item and its claims by itemId
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("claims/manager/item/{itemId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemClaimReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAllClaimsByItemIdForManager(int itemId)
        {
            //Return an item with claims instead of only own claims  
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            string[] roles = { "Manager", "Storage Manager" };
            await _firebaseAuthService.CheckUserRoles(userId, roles);
            return ResponseFactory.Ok(await _itemService.GetAnItemWithClaimsForManager(itemId));
        }

        /// <summary>
        /// (No role) Get all items that has a user has claimed, by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("claims/member/all/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<List<ItemReadWithClaimStatusDTO>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAllClaimsByUserId(string userId)
        {
            return ResponseFactory.Ok(await _itemService.GetClaimsForMember(userId));
        }

        /// <summary>
        /// (For member) Get all items that has been claimed by this member
        /// </summary>
        /// <returns></returns>
        [HttpGet("claims/my/")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<List<ItemReadWithClaimStatusDTO>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAllClaimsByCurrentUser()
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;

            return ResponseFactory.Ok(await _itemService.GetClaimsForMember(userId));
        }

        /// <summary>
        /// (For manager) Get all items that has been claimed
        /// </summary>
        /// <returns></returns>
        [HttpGet("claims/manager/all/")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemReadWithClaimStatusDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAllClaimsForManager()
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            string[] roles = { "Manager", "Storage Manager" };
            await _firebaseAuthService.CheckUserRoles(userId, roles);
            return ResponseFactory.Ok(await _itemService.GetAllClaimsForManager());
        }

        /// <summary>
        /// Update Item Cabinet
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="cabinetId"></param>
        /// <remarks>Update Item's status</remarks>
        /// <returns></returns>
        [HttpPatch("change-cabinet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateItemStatus([Required] int itemId, [Required] int cabinetId)
        {
            var result = await _itemService.UpdateItemCabinet(itemId, cabinetId);
            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// (For item founder) Get all claims of an item, without the item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("claims/all/{itemId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ItemClaimWithUserReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetClaimsWithoutItemByItemId(int itemId)
        {

            string currentUserId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            if (await _itemService.CheckItemFounderAsync(itemId, currentUserId))
            {
                var claims = await _itemClaimService.GetClaimsWithUserByItemIdAsync(itemId);
                return ResponseFactory.Ok(claims);
            }
            else throw new ItemFounderNotMatchException();
        }
    }
}
