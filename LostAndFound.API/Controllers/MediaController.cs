using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LostAndFound.API.Attributes;

namespace LostAndFound.API.Controllers
{
    [Route("api/medias")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        /// <summary>
        /// Query Media with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<MediaReadDTO>>))]
        public async Task<IActionResult> Query([FromQuery] MediaQuery query)
        {
            var paginatedMediaDTO = await _mediaService.QueryMediaAsync(query);

            return ResponseFactory.PaginatedOk(paginatedMediaDTO);
        }

        /// <summary>
        /// Query Media ignore status with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        [QueryResponseCache(typeof(MediaQueryWithStatus))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<IEnumerable<MediaDetailReadDTO>>))]
        public async Task<IActionResult> QueryIgnoreStatus([FromQuery] MediaQueryWithStatus query)
        {
            var paginatedMediaDTO = await _mediaService.QueryMediaIgnoreStatusAsync(query);

            return ResponseFactory.PaginatedOk(paginatedMediaDTO);
        }

        /// <summary>
        /// Get Media by Id
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        [HttpGet("{mediaId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<MediaReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetMediaById([Required] Guid mediaId)
        {
            var media = await _mediaService.FindMediaById(mediaId);

            return ResponseFactory.Ok(media);
        }

        /// <summary>
        /// Update Media detail
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        [HttpPut("{mediaId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<MediaReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateMedia([Required] Guid mediaId, MediaUpdateWriteDTO mediaUpdateWriteDTO)
        {
            var media = await _mediaService.UpdateMediaDetail(mediaId, mediaUpdateWriteDTO);
            return ResponseFactory.Ok(media);
        }

        /// <summary>
        /// Update Media status
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        [HttpPatch("{mediaId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateMediaStatus([Required] Guid mediaId)
        {
            await _mediaService.UpdateMediaStatus(mediaId);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Delete Media file (soft)
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        [HttpDelete("{mediaId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteMedia([Required] Guid mediaId)
        {
            await _mediaService.DeleteMediaAsync(mediaId);
            return ResponseFactory.NoContent();
        }
    }
}
