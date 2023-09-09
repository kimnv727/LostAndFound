using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface ICommentService
    {
        Task UpdateCommentStatusAsync(int commentId);
        Task DeleteCommentAsync(int commentId);
        Task<CommentReadDTO> GetCommentByIdAsync(int commentId);
        Task<PaginatedResponse<CommentReadDTO>> GetAllChildCommentByCommentIdAsync(int commentId);
        Task<PaginatedResponse<CommentReadDTO>> GetAllCommentByPostIdAsync(int postId);
        Task<PaginatedResponse<CommentReadDTO>> QueryCommentAsync(CommentQuery query);
        Task<PaginatedResponse<CommentReadDTO>> QueryCommentIgnoreStatusAsync(CommentQuery query);
        Task<CommentReadDTO> CreateCommentAsync(CommentWriteDTO commentWriteDTO);
        Task<CommentReadDTO> UpdateCommentDetailsAsync(int commentId, CommentUpdateDTO commentUpdateDTO);
    }
}