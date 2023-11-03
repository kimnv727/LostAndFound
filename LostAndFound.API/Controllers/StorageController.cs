using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.Storage;
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
    [Route("api/storages")]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        /// <summary>
        /// Get storage's details by id
        /// </summary>
        /// <param name="storageId"></param>
        /// <returns></returns>
        [HttpGet("{storageId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<StorageReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetStorage(int storageId)
        {
            var storage = await _storageService.GetStorageByIdAsync(storageId);

            return ResponseFactory.Ok(storage);
        }

        /// <summary>
        /// Get storage's details by id ignore status
        /// </summary>
        /// <param name="storageId"></param>
        /// <returns></returns>
        [HttpGet("get-ignore-status/{storageId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<StorageReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetStorageIgnoreStatus(int storageId)
        {
            var storage = await _storageService.GetStorageByIdIgnoreStatusAsync(storageId);

            return ResponseFactory.Ok(storage);
        }

        /// <summary>
        /// Get storage's details by id include cabinet
        /// </summary>
        /// <param name="storageId"></param>
        /// <returns></returns>
        [HttpGet("get-include-cabinet/{storageId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<StorageReadIncludeCabinetsDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetStorageIncludeCabinets(int storageId)
        {
            var storage = await _storageService.GetStorageByIdIncludeCabinetsAsync(storageId);

            return ResponseFactory.Ok(storage);
        }

        /// <summary>
        /// Get storage's details by id ignore status include cabinet
        /// </summary>
        /// <param name="storageId"></param>
        /// <returns></returns>
        [HttpGet("get-ignore-status-include-cabinet/{storageId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<StorageReadIncludeCabinetsDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetStorageIgnoreStatusIncludeCabinets(int storageId)
        {
            var storage = await _storageService.GetStorageByIdIncludeCabinetsIgnoreStatusAsync(storageId);

            return ResponseFactory.Ok(storage);
        }

        /// <summary>
        /// Get all storages by campusId (only Active)
        /// </summary>
        /// <param name="campusId"></param>
        /// <returns></returns>
        [HttpGet("get-all-by-campus/{campusId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<StorageReadDTO[]>))]
        public async Task<IActionResult> GetAllStoragesByCampusId(int campusId)
        {
            var result = await _storageService.GetAllStoragesByCampusIdAsync(campusId);

            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get all storages by campusId ignore status
        /// </summary>
        /// <param name="campusId"></param>
        /// <returns></returns>
        [HttpGet("get-all-by-campus-ignore-status/{campusId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<StorageReadDTO[]>))]
        public async Task<IActionResult> GetAllStoragesByCampusIdIgnoreStatus(int campusId)
        {
            var result = await _storageService.GetAllStoragesByCampusIdIgnoreStatusAsync(campusId);

            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get all storages by campusId include cabinets (only Active)
        /// </summary>
        /// <param name="campusId"></param>
        /// <returns></returns>
        [HttpGet("get-all-by-campus-include-cabinet/{campusId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<StorageReadIncludeCabinetsDTO[]>))]
        public async Task<IActionResult> GetAllStoragesByCampusIdIncludeCabinet(int campusId)
        {
            var result = await _storageService.GetAllStoragesByCampusIdIncludeCabinetsAsync(campusId);

            return ResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get all storages by campusId ignore status include cabinets
        /// </summary>
        /// <param name="campusId"></param>
        /// <returns></returns>
        [HttpGet("get-all-by-campus-ignore-status-include-cabinet/{campusId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<StorageReadIncludeCabinetsDTO[]>))]
        public async Task<IActionResult> GetAllStoragesByCampusIdIgnoreStatusIncludeCabinet(int campusId)
        {
            var result = await _storageService.GetAllStoragesByCampusIdIgnoreStatusIncludeCabinetsAsync(campusId);

            return ResponseFactory.Ok(result);
        }

        ///<summary>
        /// Query Storage
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<StorageReadIncludeCabinetsDTO>))]
        public async Task<IActionResult> QueryStorages([FromQuery] StorageQuery query)
        {
            var storagePaginatedDto = await _storageService.QueryStorageAsync(query);

            return ResponseFactory.PaginatedOk(storagePaginatedDto);
        }

        ///<summary>
        /// Create new storage
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<StorageReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateStorage([FromForm] StorageWriteDTO writeDTO)
        {
            var result = await _storageService.CreateStorageAsync(writeDTO);

            return ResponseFactory.CreatedAt(
                (nameof(GetStorage)),
                nameof(StorageController),
                new { storageId = result.Id },
                result);
        }

        /// <summary>
        /// Update storage detail
        /// </summary>
        /// <param name="storageId"></param>
        /// <returns></returns>
        [HttpPut("{storageId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<StorageReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateStorage(int storageId, StorageUpdateDTO storageUpdateDTO)
        {
            //check Roles
            //check Authorization 
            var storage = await _storageService.UpdateStorageDetailsAsync(storageId, storageUpdateDTO);
            return ResponseFactory.Ok(storage);
        }

        /// <summary>
        /// Update Storage Status
        /// </summary>
        /// <remarks></remarks>
        /// <param name="storageId"></param>
        /// <returns></returns>
        [HttpPatch("status/{storageId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateStorageStatus(int storageId)
        {
            await _storageService.UpdateStorageStatusAsync(storageId);
            return ResponseFactory.NoContent();
        }
    }
}
