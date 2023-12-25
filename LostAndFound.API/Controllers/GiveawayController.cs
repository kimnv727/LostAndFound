using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Giveaway;
using LostAndFound.Infrastructure.DTOs.GiveawayParticipant;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.API.Controllers
{
    [ApiController]
    [Route("api/giveaways")]
    public class GiveawayController : ControllerBase
    {
        private readonly IGiveawayService _giveawayService;
        private readonly IGiveawayParticipantService _giveawayParticipantService;

        public GiveawayController(IGiveawayService giveawayService, IGiveawayParticipantService giveawayParticipantService)
        {
            _giveawayService = giveawayService;
            _giveawayParticipantService = giveawayParticipantService;
        }
        
        /// <summary>
        /// Get giveaway's details by id
        /// </summary>
        /// <param name="giveawayId"></param>
        /// <returns></returns>
        [HttpGet("{giveawayId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<GiveawayReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetGiveawayById(int giveawayId)
        {
            var giveaway = await _giveawayService.GetGiveawayByIdAsync(giveawayId);

            return ResponseFactory.Ok(giveaway);
        }
        
        /// <summary>
        /// Get giveaway's details include Participants by id
        /// </summary>
        /// <param name="giveawayId"></param>
        /// <returns></returns>
        [HttpGet("get-with-participants/{giveawayId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<GiveawayDetailWithParticipantsReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetGiveawayIncludeParticipantsById(int giveawayId)
        {
            var giveaway = await _giveawayService.GetGiveawayIncludeParticipantsByIdAsync(giveawayId);

            return ResponseFactory.Ok(giveaway);
        }
        
        ///<summary>
        /// Get all giveaways
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<GiveawayReadDTO>))]
        public async Task<IActionResult> GetAllGiveaways([FromQuery] GiveawayQuery query)
        {
            var giveawayPaginatedDto = await _giveawayService.QueryGiveawayAsync(query);

            return ResponseFactory.PaginatedOk(giveawayPaginatedDto);
        }
        
        ///<summary>
        /// Get all giveaways with status
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query-with-status")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<GiveawayReadDTO>))]
        public async Task<IActionResult> GetAllGiveawaysWithStatus([FromQuery] GiveawayQueryWithStatus query)
        {
            var giveawayPaginatedDto = await _giveawayService.QueryGiveawayWithStatusAsync(query);

            return ResponseFactory.PaginatedOk(giveawayPaginatedDto);
        }
        
        ///<summary>
        /// Create new giveaway
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<GiveawayReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateGiveaway(GiveawayWriteDTO writeDTO)
        {
            var result = await _giveawayService.CreateGiveawayAsync(writeDTO);

            return ResponseFactory.CreatedAt(
                (nameof(GetGiveawayById)), 
                nameof(GiveawayController), 
                new { giveawayId = result.Id }, 
                result);
        }
        
        /// <summary>
        /// Update giveaway detail
        /// </summary>
        /// <param name="giveawayId"></param>
        /// <returns></returns>
        [HttpPut("{giveawayId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<GiveawayReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateGiveaway(int giveawayId, GiveawayUpdateDTO giveawayUpdateDTO)
        {
            var giveaway = await _giveawayService.UpdateGiveawayDetailsAsync(giveawayId, giveawayUpdateDTO);
            return ResponseFactory.Ok(giveaway);
        }
        
        /// <summary>
        /// Update Giveaway Status
        /// </summary>
        /// <remarks></remarks>
        /// <param name="giveawayId"></param>
        /// <returns></returns>
        [HttpPatch("status/{giveawayId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<GiveawayReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateGiveawayStatus(int giveawayId, GiveawayStatus giveawayStatus)
        {
            //TODO: add more check (like not allowed to change back to NOT STARTED)
            await _giveawayService.UpdateGiveawayStatusAsync(giveawayId, giveawayStatus);
            var giveaway = await _giveawayService.GetGiveawayByIdAsync(giveawayId);
            return ResponseFactory.Ok(giveaway);
        }
        
        /// <summary>
        /// Get total number of Participants of a Giveaway
        /// </summary>
        /// <param name="giveawayId"></param>
        /// <returns></returns>
        [HttpGet("count-giveaway-participants/{giveawayId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> CountGiveawayParticipantsAsync(int giveawayId)
        {
            return ResponseFactory.Ok(await _giveawayParticipantService.CountGiveawayParticipantsAsync(giveawayId));
        }
        
        /// <summary>
        /// Get GiveawayParticipant Detail
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="giveawayId"></param>
        /// <returns></returns>
        [HttpGet("get-giveaway-participant")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<GiveawayParticipantReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetGiveawayParticipant(string userId, int giveawayId)
        {
            var giveawayParticipant = await _giveawayParticipantService.GetGiveawayParticipant(giveawayId, userId);

            return ResponseFactory.Ok(giveawayParticipant);
        }
        
        /// <summary>
        /// Get all Participants of A Giveaway
        /// </summary>
        /// <param name="giveawayId"></param>
        /// <returns></returns>
        [HttpGet("get-all-giveaway-participants")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UserReadDTO[]>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetUsersParticipateInGiveaway(int giveawayId)
        {
            return ResponseFactory.Ok(await _giveawayParticipantService.GetUsersParticipateInGiveaway(giveawayId));
        }
        
        /// <summary>
        /// Participate in a Giveaway
        /// </summary>
        /// <param name="giveawayId"></param>
        /// <returns></returns>
        [HttpPost("participate-in-a-giveaway/{giveawayId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<GiveawayParticipantReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ParticipateInGiveaway(int giveawayId)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var giveawayParticipant = await _giveawayParticipantService.ParticipateInGiveaway(stringId, giveawayId);
            
            return ResponseFactory.CreatedAt(nameof(GetGiveawayParticipant), 
                nameof(GiveawayController), 
                new { userId = giveawayParticipant.User.Id, giveawayId = giveawayParticipant.GiveawayId }, 
                giveawayParticipant);
        }

        /// <summary>
        /// List items suitable for Giveaway
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-suitable-items-for-giveaway")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<ItemReadDTO>>))]
        public async Task<IActionResult> ListItemsSortByFloorNumber()
        {
            var items = await _giveawayService.ListItemsSuitableForGiveawayAsync();

            return ResponseFactory.Ok(items);
        }
    }
}