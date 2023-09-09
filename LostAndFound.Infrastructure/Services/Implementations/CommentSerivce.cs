using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.Services.Interfaces;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class CommentSerivce : ICommentService
    {
        public Task UpdateCommentStatusAsync(int commentId)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteCommentAsync(int commentId)
        {
            throw new System.NotImplementedException();
        }

        public Task<CommentReadDTO> GetCommentByIdAsync(int commentId)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedResponse<CommentReadDTO>> GetAllChildCommentByCommentIdAsync(int commentId)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedResponse<CommentReadDTO>> GetAllCommentByPostIdAsync(int postId)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedResponse<CommentReadDTO>> QueryCommentAsync(CommentQuery query)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedResponse<CommentReadDTO>> QueryCommentIgnoreStatusAsync(CommentQuery query)
        {
            throw new System.NotImplementedException();
        }

        public Task<CommentReadDTO> CreateCommentAsync(CommentWriteDTO commentWriteDTO)
        {
            throw new System.NotImplementedException();
        }

        public Task<CommentReadDTO> UpdateCommentDetailsAsync(int commentId, CommentUpdateDTO commentUpdateDTO)
        {
            throw new System.NotImplementedException();
        }
    }
}