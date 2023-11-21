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
        public async Task<IActionResult> Query([FromQuery] ReceiptQuery query)
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<ReceiptReadDTO>>))]
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
        public async Task<ReceiptReadDTO> CreateReceiptAsync([FromForm] ReceiptCreateDTO receiptCreateDTO, [Required] IFormFile image)
        {
            string userId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            string[] roles = { "User", "Storage Manager" };
            await _firebaseAuthService.CheckUserRoles(userId, roles);

            var receipt = await _receiptService.CreateReceiptAsync(receiptCreateDTO, image);
            return _mapper.Map<ReceiptReadDTO>(receipt);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ReceiptReadDTO>))]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ReceiptReadDTO[]>))]
        public async Task<IActionResult> GetAllCabinetsByStorageId(int itemId)
        {
            var result = await _receiptService.GetAllReceiptsByItemIdAsync(itemId);

            return ResponseFactory.Ok(result);
        }
    }
}
