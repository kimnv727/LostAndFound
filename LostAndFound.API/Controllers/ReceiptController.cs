using LostAndFound.API.ResponseWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Receipt;
using LostAndFound.Infrastructure.Services.Interfaces;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using LostAndFound.Infrastructure.Services.Implementations;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Location;
using System.Collections.Generic;
using LostAndFound.API.Authentication;

namespace LostAndFound.API.Controllers
{
    [Route("api/receipts")]
    [ApiController]
    public class ReceiptController : Controller
    {
        private readonly IReceiptService _receiptService;
        private readonly IMapper _mapper;
        private readonly IFirebaseAuthService _firebaseAuthService;

        public ReceiptController(IReceiptService receiptService, IMapper mapper, IFirebaseAuthService firebaseAuthService)
        {
            _receiptService = receiptService;
            _mapper = mapper;
            _firebaseAuthService = firebaseAuthService;
        }

        /// <summary>
        /// Query receipts paginated
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> Query([FromQuery] TransferRecordQuery query)
        {
            var paginatedReceiptDTO = await _receiptService.QueryReceiptAsync(query);

            return ResponseFactory.PaginatedOk(paginatedReceiptDTO);
        }

        /// <summary>
        /// List receipts
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<TransferRecordReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ListAll()
        {
            var receipts = await _receiptService.ListAllAsync();

            return ResponseFactory.Ok(receipts);
        }

        /// <summary>
        /// Create new receipt
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> CreateReceiptAsync([FromForm] TransferRecordCreateDTO receiptCreateDTO, [Required] IFormFile image)
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            string[] roles = { "User", "Storage Manager" };
            await _firebaseAuthService.CheckUserRoles(userId, roles);

            var receipt = await _receiptService.CreateReceiptAsync(receiptCreateDTO, image);
            return ResponseFactory.Ok(_mapper.Map<TransferRecordReadDTO>(receipt));
        }

        /// <summary>
        /// Create new receipt for giveaway
        /// </summary>
        /// <returns></returns>
        [HttpPost("giveaway")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> CreateReceiptForGiveawayAsync([FromForm] TransferRecordGiveawayCreateDTO receiptCreateDTO, [Required] IFormFile image)
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            string[] roles = { "Storage Manager" };
            await _firebaseAuthService.CheckUserRoles(userId, roles);

            var receipt = await _receiptService.CreateReceiptForGiveawayAsync(userId, receiptCreateDTO, image);
            return ResponseFactory.Ok(_mapper.Map<TransferRecordReadDTO>(receipt));
        }

        /// <summary>
        /// Create new receipt for onhold Item
        /// </summary>
        /// <returns></returns>
        [HttpPost("onhold-item")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> CreateReceiptForOnholdItemAsync([FromForm] TransferRecordOnholdItemCreateDTO receiptCreateDTO, [Required] IFormFile image)
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            string[] roles = { "Storage Manager" };
            await _firebaseAuthService.CheckUserRoles(userId, roles);

            var receipt = await _receiptService.CreateReceiptForOnHoldItemAsync(userId, receiptCreateDTO, image);
            return ResponseFactory.Ok(_mapper.Map<TransferRecordReadDTO>(receipt));
        }

        /// <summary>
        /// Delete a receipt and its media
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{receiptId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteReceiptAsync([Required] int receiptId)
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            string[] roles = { "User", "Storage Manager" };

            await _firebaseAuthService.CheckUserRoles(userId, roles);
            await _receiptService.DeleteReceiptAsync(receiptId);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Get receipt by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id/{receiptId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<TransferRecordReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetReceiptByIdAsync([Required] int receiptId)
        {
            var receipt = await _receiptService.FindReceiptByIdAsync(receiptId);

            return ResponseFactory.Ok(receipt);
        }

        /// <summary>
        /// Get all receipt by itemId 
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("get-all-by-item/{itemId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<TransferRecordReadDTO[]>))]
        public async Task<IActionResult> GetAllReceiptsByItemId(int itemId)
        {
            var result = await _receiptService.GetAllReceiptsByItemIdAsync(itemId);

            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get all receipt by itemId 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-by-user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<TransferRecordReadDTO[]>))]
        public async Task<IActionResult> GetAllReceiptsByUserId()
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _receiptService.GetReceiptsByUserIdAsync(userId);

            return ResponseFactory.Ok(result);
        }
    }
}
