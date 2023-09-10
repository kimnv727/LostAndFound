using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LostAndFound.API.Attributes;
using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.DTOs.Comment;
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
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
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
        /// Get comment's details with reply by id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpGet("get-with-reply/{commentId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CommentDetailWithReplyDetailReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetCommentWithReply(int commentId)
        {
            var comment = await _commentService.GetCommentWithReplyByIdAsync(commentId);

            return ResponseFactory.Ok(comment);
        }
        
        /// <summary>
        /// Get all comment by postId
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("get-by-post/{postId}")]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [QueryResponseCache(typeof(CommentQuery))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<CommentReadDTO>))]
        public async Task<IActionResult> GetAllComments([FromQuery] CommentQuery query)
        {
            var commentPaginatedDto = await _commentService.QueryCommentAsync(query);

            return ResponseFactory.PaginatedOk(commentPaginatedDto);
        }
        
        ///<summary>
        /// Get all comments
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-ignore-status")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [QueryResponseCache(typeof(CommentQuery))]
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
                new { id = result.Id }, 
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
                new { id = result.Id }, 
                result);
        }
        
        /// <summary>
        /// Update comment
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpPut("{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<CommentReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateComment(int commentId, CommentUpdateDTO commentUpdateWriteDTO)
        {
            var comment = await _commentService.UpdateCommentDetailsAsync(commentId, commentUpdateWriteDTO);
            return ResponseFactory.Ok(comment);
        }
        
        /// <summary>
        /// Change comment's status
        /// </summary>
        /// <remarks></remarks>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpPatch("{commentId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ChangeCommentStatus(int commentId)
        {
            await _commentService.UpdateCommentStatusAsync(commentId);

            return ResponseFactory.NoContent();
        }
        
        /// <summary>
        /// Delete Comment (soft)
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpDelete("{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteComment([Required] int commentId)
        {
            await _commentService.DeleteCommentAsync(commentId);
            return ResponseFactory.NoContent();
        }
    }
}