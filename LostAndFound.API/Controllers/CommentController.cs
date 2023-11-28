using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Authenticate;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.DTOs.CommentFlag;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.API.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ICommentFlagService _commentFlagService;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IUserService _userService;

        public CommentController(ICommentService commentService, ICommentFlagService commentFlagService, IFirebaseAuthService firebaseAuthService,
            IUserService userService)
        {
            _commentService = commentService;
            _commentFlagService = commentFlagService;
            _firebaseAuthService = firebaseAuthService;
            _userService = userService;
        }
        
        /// <summary>
        /// Get comment's details by id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpGet("{commentId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CommentDetailReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetComment(int commentId)
        {
            var comment = await _commentService.GetCommentByIdAsync(commentId);

            return ResponseFactory.Ok(comment);
        }

        /// <summary>
        /// Get all comment by postId
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("get-by-post/{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<CommentReadDTO>))]
        public async Task<IActionResult> GetAllCommentsByPostId([Required] int postId)
        {
            var result = await _commentService.GetAllCommentByPostIdAsync(postId);

            return ResponseFactory.PaginatedOk(result);
        }
        
        ///<summary>
        /// Get all comments
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<CommentReadDTO>))]
        public async Task<IActionResult> GetAllComments([FromQuery] CommentQuery query)
        {
            var commentPaginatedDto = await _commentService.QueryCommentAsync(query);

            return ResponseFactory.PaginatedOk(commentPaginatedDto);
        }
        
        ///<summary>
        /// Get all comments ignore status
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-ignore-status")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<CommentReadDTO>))]
        public async Task<IActionResult> GetAllCommentsIgnoreStatus([FromQuery] CommentQuery query)
        {
            var commentPaginatedDto = await _commentService.QueryCommentIgnoreStatusAsync(query);

            return ResponseFactory.PaginatedOk(commentPaginatedDto);
        }
        
        ///<summary>
        /// Create new comment (reply to post)
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost("reply-post/{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<CommentReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateComment([Required] int postId, CommentWriteDTO writeDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _commentService.CreateCommentAsync(stringId, postId, writeDTO);

            return ResponseFactory.CreatedAt(
                (nameof(GetComment)), 
                nameof(CommentController), 
                new { commentId = result.Id }, 
                result);
        }
        
        ///<summary>
        /// Create new comment (reply to comment)
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost("reply-comment/{commentId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<CommentReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> ReplyComment([Required] int commentId, CommentWriteDTO writeDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _commentService.ReplyToCommentAsync(stringId, commentId, writeDTO);

            return ResponseFactory.CreatedAt(
                (nameof(GetComment)), 
                nameof(CommentController), 
                new { commentId = result.Id }, 
                result);
        }
        
        /// <summary>
        /// Update comment
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpPut("{commentId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CommentReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateComment(int commentId, CommentUpdateDTO commentUpdateWriteDTO)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            var commentCheck = await _commentService.GetCommentByIdAsync(commentId);
            if (stringId != commentCheck.CommentUserId)
            {
                throw new UnauthorizedException();
            }
            var comment = await _commentService.UpdateCommentDetailsAsync(commentId, commentUpdateWriteDTO);
            return ResponseFactory.Ok(comment);
        }

        /// <summary>
        /// Delete Comment (soft)
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpDelete("{commentId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteComment([Required] int commentId)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;

            var commentCheck = await _commentService.GetCommentByIdAsync(commentId);
            var currentUser = await _userService.GetUserAsync(stringId);
            if(currentUser.RoleName != "Admin" && currentUser.RoleName != "Manager")
            {
                if (stringId != commentCheck.CommentUserId)
                {
                    throw new NotPermittedException("You are not permitted to delete this comment!");
                }
            }
            
            await _commentService.DeleteCommentAsync(commentId);
            return ResponseFactory.NoContent();
        }
        
        /// <summary>
        /// Get total number of Comment Flag of a Comment
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpGet("count-comment-flag/{commentId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CommentFlagCountReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> CountCommentFlagOfAPost(int commentId)
        {
            return ResponseFactory.Ok(await _commentFlagService.CountCommentFlagAsync(commentId));
        }
        
        /// <summary>
        /// Get Comment Flag Detail
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpGet("get-comment-flag")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CommentFlagReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetCommentFlag(string userId, int commentId)
        {
            var commentFlag = await _commentFlagService.GetCommentFlag(userId, commentId);

            return ResponseFactory.Ok(commentFlag);
        }
        
        /// <summary>
        /// Get all own CommentFlags
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-own-comment-flag")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CommentReadDTO[]>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetAllOwnCommentFlag()
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            return ResponseFactory.Ok(await _commentFlagService.GetOwnCommentFlags(stringId));
        }
        
        /// <summary>
        /// Flag A Comment
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpPost("flag-a-comment/{commentId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CommentFlagReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> FlagAComment(int commentId, [FromForm] CommentFlagReason reason)
        {
            string stringId = User.Claims.First(clm => clm.Type == ClaimTypes.NameIdentifier).Value;
            //check cant flag your own comment
            var commentCheck = await _commentService.GetCommentByIdAsync(commentId);
            if (stringId == commentCheck.CommentUserId)
            {
                throw new NotPermittedException("You are not permitted to access this function");
            }

            //Flag a comment
            var commentFlag = await _commentFlagService.FlagAComment(stringId, commentId, reason);
            
            return ResponseFactory.CreatedAt(nameof(GetCommentFlag), 
                nameof(CommentController), 
                new { userId = commentFlag.UserId, commentId = commentFlag.Comment.Id }, 
                commentFlag);
        }

        ///<summary>
        /// Get all comments with flag
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query-with-flag")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<CommentDetailReadWithFlagDTO>))]
        public async Task<IActionResult> GetAllCommentsWithFlag([FromQuery] CommentQueryWithFlag query)
        {
            var commentPaginatedDto = await _commentService.QueryCommentWithFlagAsync(query);

            return ResponseFactory.PaginatedOk(commentPaginatedDto);
        }
    }
}