using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Authenticate;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.DTOs.PostBookmark;
using LostAndFound.Infrastructure.DTOs.PostFlag;
using LostAndFound.Infrastructure.DTOs.PostMedia;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.API.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IPostMediaService _postMediaService;
        private readonly IPostBookmarkService _postBookmarkService;
        private readonly IPostFlagService _postFlagService;

        public PostController(IPostService postService, IPostMediaService postMediaService,
            IPostBookmarkService postBookmarkService, IPostFlagService postFlagService)
        {
            _postService = postService;
            _postMediaService = postMediaService;
            _postBookmarkService = postBookmarkService;
            _postFlagService = postFlagService;
        }

        /// <summary>
        /// Get post's details by id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostDetailWithCommentsReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetPost(int postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            return ResponseFactory.Ok(post);
        }
        
        /// <summary>
        /// Get all posts by userId (only Active)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("get-by-user/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostReadDTO[]>))]
        public async Task<IActionResult> GetAllPostsByUserId(string userId)
        {
            var result = await _postService.GetPostByUserIdAsync(userId);

            return ResponseFactory.Ok(result);
        }
        
        ///<summary>
        /// Get all posts
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        //[QueryResponseCache(typeof(PostQuery))]
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
        [HttpGet("query-with-status")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        //[QueryResponseCache(typeof(PostQueryWithStatus))]
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
        public async Task<IActionResult> CreatePost([FromForm]PostWriteDTO writeDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _postService.CreatePostAsync(stringId, writeDTO);

            return ResponseFactory.CreatedAt(
                (nameof(GetPost)), 
                nameof(PostController), 
                new { postId = result.Id }, 
                result);
        }
        
        /// <summary>
        /// Update post detail
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPut("{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostDetailReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdatePost(int postId, PostUpdateDTO postUpdateWriteDTO)
        {
            //check Authorization 
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var postCheck = await _postService.GetPostByIdAsync(postId);
            if (stringId != postCheck.PostUserId)
            {
                throw new UnauthorizedException();
            }
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostDetailReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdatePoststatus(int postId, PostStatusExcludeDelete postStatusExcludeDelete)
        {
            var postStatus = PostStatus.PENDING;
            switch (postStatusExcludeDelete)
            {
                case PostStatusExcludeDelete.ACTIVE:
                    postStatus = PostStatus.ACTIVE;
                    break;
                case PostStatusExcludeDelete.CLOSED:
                    postStatus = PostStatus.CLOSED;
                    break;
                default:
                    break;
            }
            await _postService.UpdatePostStatusAsync(postId, postStatus);
            var post = await _postService.GetPostByIdAsync(postId);
            return ResponseFactory.Ok(post);
        }
        
        /// <summary>
        /// Delete Post (soft)
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpDelete("{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeletePost([Required] int postId)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var postCheck = await _postService.GetPostByIdAsync(postId);
            if (stringId != postCheck.PostUserId)
            {
                throw new UnauthorizedException();
            }
            await _postService.DeletePostAsync(postId);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Get all Post's Medias
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("{postId}/media")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostMediaReadDTO[]>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAllPostMedia([Required] int postId)
        {
            return ResponseFactory.Ok(await _postMediaService.GetPostMedias(postId));
        }

        /// <summary>
        /// Upload Post's media files
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("{postId}/media")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostMediaReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> CreatePostMedias([Required] int postId, [Required] IFormFile[] files)
        {
            var postMedias = await _postMediaService.UploadPostMedias(User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value, postId, files);
            return ResponseFactory.CreatedAt(nameof(GetAllPostMedia), 
                                            nameof(PostController), 
                                            new { postId = postId }, 
                                            postMedias);
        }

        /// <summary>
        /// Delete Post's media file (soft)
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        [HttpDelete("{postId}/media")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteMedia([Required] int postId, [Required] Guid mediaId)
        {
            await _postMediaService.DeletePostMedia(User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value, postId, mediaId);
            return ResponseFactory.NoContent();
        }
        
        /// <summary>
        /// Get total number of Post Bookmark of a Post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("count-post-bookmark/{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> CountPostBookmarkOfAPost(int postId)
        {
            return ResponseFactory.Ok(await _postBookmarkService.CountPostBookmarkAsync(postId));
        }
        
        /// <summary>
        /// Get Post Bookmark Detail
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("get-post-bookmark")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostBookmarkReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetPostBookmark(string userId, int postId)
        {
            var postBookmark = await _postBookmarkService.GetPostBookmark(userId, postId);

            return ResponseFactory.Ok(postBookmark);
        }
        
        /// <summary>
        /// Get all own PostBookmarks
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-own-post-bookmark")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostReadDTO[]>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAllOwnPostBookmark()
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            return ResponseFactory.Ok(await _postBookmarkService.GetOwnPostBookmarkeds(stringId));
        }
        
        /// <summary>
        /// Bookmark A Post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPost("bookmark-a-post/{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostBookmarkReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> BookmarkAPost(int postId)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var postBookmark = await _postBookmarkService.BookmarkAPost(stringId, postId);
            
            return ResponseFactory.CreatedAt(nameof(GetPostBookmark), 
                nameof(PostController), 
                new { userId = postBookmark.UserId, postId = postBookmark.Post.Id }, 
                postBookmark);
        }
        
        /// <summary>
        /// Get total number of Post Flag of a Post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("count-post-flag/{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> CountPostFlagOfAPost(int postId)
        {
            return ResponseFactory.Ok(await _postFlagService.CountPostFlagAsync(postId));
        }
        
        /// <summary>
        /// Get Post Flag Detail
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("get-post-flag")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostFlagReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetPostFlag(string userId, int postId)
        {
            var postFlag = await _postFlagService.GetPostFlag(userId, postId);

            return ResponseFactory.Ok(postFlag);
        }
        
        /// <summary>
        /// Get all own PostFlags
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-own-post-flag")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostReadDTO[]>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAllOwnPostFlag()
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            return ResponseFactory.Ok(await _postFlagService.GetOwnPostFlags(stringId));
        }
        
        /// <summary>
        /// Flag A Post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPost("flag-a-post/{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PostFlagReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FlagAPost(int postId, PostFlagReason reason)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var postFlag = await _postFlagService.FlagAPost(stringId, postId, reason);
            
            return ResponseFactory.CreatedAt(nameof(GetPostFlag), 
                nameof(PostController), 
                new { userId = postFlag.UserId, postId = postFlag.Post.Id }, 
                postFlag);
        }
    }
}