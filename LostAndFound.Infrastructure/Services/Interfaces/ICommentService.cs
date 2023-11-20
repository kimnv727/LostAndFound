using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface ICommentService
    {
        //Task UpdateCommentStatusAsync(int commentId);
        Task DeleteCommentAsync(int commentId);
        Task<CommentDetailReadDTO> GetCommentByIdAsync(int commentId);
        Task<CommentDetailReadDTO> GetCommentIgnoreStatusByIdAsync(int commentId);
        //Task<CommentDetailWithReplyDetailReadDTO> GetCommentWithReplyByIdAsync(int commentId);
        Task<PaginatedResponse<CommentReadDTO>> GetAllCommentByPostIdAsync(int postId);
        Task<PaginatedResponse<CommentReadDTO>> QueryCommentAsync(CommentQuery query);
        Task<PaginatedResponse<CommentReadDTO>> QueryCommentIgnoreStatusAsync(CommentQuery query);
        Task<CommentReadDTO> CreateCommentAsync(string userId, int postId, CommentWriteDTO commentWriteDTO);
        Task<CommentReadDTO> ReplyToCommentAsync(string userId, int commentId, CommentWriteDTO commentWriteDTO);
        Task<CommentReadDTO> UpdateCommentDetailsAsync(int commentId, CommentUpdateDTO commentUpdateDTO);
        Task<bool> CheckCommentAuthorAsync(int commentId, string userId);
        Task<PaginatedResponse<CommentDetailReadWithFlagDTO>> QueryCommentWithFlagAsync(CommentQueryWithFlag query);
    }
}