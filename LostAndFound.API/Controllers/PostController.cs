using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        
        /// <summary>
        /// Get post's details by id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("{postId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostDetailWithCommentsReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetPost(int postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            return ResponseFactory.Ok(post);
        }
        
        /// <summary>
        /// Get all posts by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<PostReadDTO>))]
        public async Task<IActionResult> GetAllPostsByUserId(string userId)
        {
            var result = await _postService.GetPostByUserIdAsync(userId);

            return ResponseFactory.PaginatedOk(result);
        }
        
        ///<summary>
        /// Get all posts
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [QueryResponseCache(typeof(PostQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<PostReadDTO>))]
        public async Task<IActionResult> GetAllPosts([FromQuery] PostQuery query)
        {
            var postPaginatedDto = await _postService.QueryPostAsync(query);

            return ResponseFactory.PaginatedOk(postPaginatedDto);
        }
        
        ///<summary>
        /// Get all posts with status
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-with-status")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [QueryResponseCache(typeof(PostQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<PostDetailReadDTO>))]
        public async Task<IActionResult> GetAllPostsWithStatus([FromQuery] PostQueryWithStatus query)
        {
            var postPaginatedDto = await _postService.QueryPostWithStatusAsync(query);

            return ResponseFactory.PaginatedOk(postPaginatedDto);
        }
        
        ///<summary>
        /// Create new post
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<PostDetailReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreatePost(PostWriteDTO writeDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _postService.CreatePostAsync(stringId, writeDTO);

            return ResponseFactory.CreatedAt(
                (nameof(GetPost)), 
                nameof(PostController), 
                new { id = result.Id }, 
                result);
        }
        
        /// <summary>
        /// Update post detail
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPut("{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostDetailReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdatePost(int postId, PostUpdateDTO postUpdateWriteDTO)
        {
            var post = await _postService.UpdatePostDetailsAsync(postId, postUpdateWriteDTO);
            return ResponseFactory.Ok(post);
        }
        
        /// <summary>
        /// Update Post Status
        /// </summary>
        /// <remarks></remarks>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPatch("status/{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdatePoststatus(int postId, PostStatus postStatus)
        {
            await _postService.UpdatePostStatusAsync(postId, postStatus);

            return ResponseFactory.NoContent();
        }
        
        /// <summary>
        /// Delete Post (soft)
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpDelete("{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeletePost([Required] int postId)
        {
            await _postService.DeletePostAsync(postId);
            return ResponseFactory.NoContent();
        }
    }
}