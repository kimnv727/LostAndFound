using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.Cabinet;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.API.Controllers
{
    [ApiController]
    [Route("api/cabinets")]
    public class CabinetController : ControllerBase
    {
        private readonly ICabinetService _cabinetService;

        public CabinetController(ICabinetService cabinetService)
        {
            _cabinetService = cabinetService;
        }

        /// <summary>
        /// Get cabinet's details by id
        /// </summary>
        /// <param name="cabinetId"></param>
        /// <returns></returns>
        [HttpGet("{cabinetId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CabinetReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetCabinet(int cabinetId)
        {
            var cabinet = await _cabinetService.GetCabinetByIdAsync(cabinetId);

            return ResponseFactory.Ok(cabinet);
        }

        /// <summary>
        /// Get cabinet's details by id ignore status
        /// </summary>
        /// <param name="cabinetId"></param>
        /// <returns></returns>
        [HttpGet("get-ignore-status/{cabinetId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CabinetReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetCabinetIgnoreStatus(int cabinetId)
        {
            var cabinet = await _cabinetService.GetCabinetByIdIgnoreStatusAsync(cabinetId);

            return ResponseFactory.Ok(cabinet);
        }

        /// <summary>
        /// Get all cabinets by storageId (only Active)
        /// </summary>
        /// <param name="storageId"></param>
        /// <returns></returns>
        [HttpGet("get-all-by-storage/{storageId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CabinetReadDTO[]>))]
        public async Task<IActionResult> GetAllCabinetsByStorageId(int storageId)
        {
            var result = await _cabinetService.GetAllCabinetsByStorageIdAsync(storageId);

            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get all cabinets by storageId ignore sttaus
        /// </summary>
        /// <param name="storageId"></param>
        /// <returns></returns>
        [HttpGet("get-all-by-storage-ignore-status/{storageId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CabinetReadDTO[]>))]
        public async Task<IActionResult> GetAllCabinetsByStorageIdIgnoreStatus(int storageId)
        {
            var result = await _cabinetService.GetAllCabinetsByStorageIdIgnoreStatusAsync(storageId);

            return ResponseFactory.Ok(result);
        }

        ///<summary>
        /// Query Cabinet
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<CabinetReadDTO>))]
        public async Task<IActionResult> QueryCabinets([FromQuery] CabinetQuery query)
        {
            var cabinetPaginatedDto = await _cabinetService.QueryCabinetAsync(query);

            return ResponseFactory.PaginatedOk(cabinetPaginatedDto);
        }

        ///<summary>
        /// Create new cabinet
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<CabinetReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateCabinet(CabinetWriteDTO writeDTO)
        {
            var result = await _cabinetService.CreateCabinetAsync(writeDTO);

            return ResponseFactory.CreatedAt(
                (nameof(GetCabinet)),
                nameof(CabinetController),
                new { cabinetId = result.Id },
                result);
        }

        /// <summary>
        /// Update cabinet detail
        /// </summary>
        /// <param name="cabinetId"></param>
        /// <returns></returns>
        [HttpPut("{cabinetId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CabinetReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateCabinet(int cabinetId, CabinetUpdateDTO cabinetUpdateDTO)
        {
            //check Roles
            //check Authorization 
            var cabinet = await _cabinetService.UpdateCabinetDetailsAsync(cabinetId, cabinetUpdateDTO);
            return ResponseFactory.Ok(cabinet);
        }

        /// <summary>
        /// Update Cabinet Status
        /// </summary>
        /// <remarks></remarks>
        /// <param name="cabinetId"></param>
        /// <returns></returns>
        [HttpPatch("status/{cabinetId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CabinetReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateCabinetStatus(int cabinetId)
        {
            return ResponseFactory.Ok(await _cabinetService.UpdateCabinetStatusAsync(cabinetId));
        }

        /// <summary>
        /// Get all cabinets no paginated (only Active)
        /// </summary>
        /// <returns></returns>
        [HttpGet("list-all")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CabinetReadDTO[]>))]
        public async Task<IActionResult> ListAllCabinets()
        {
            var result = await _cabinetService.ListAllCabinetsAsync();

            return ResponseFactory.Ok(result);
        }
    }
}
