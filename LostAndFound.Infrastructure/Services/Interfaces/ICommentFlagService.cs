using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.DTOs.CommentFlag;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface ICommentFlagService
    {
        Task<int> CountCommentFlagAsync(int commentId);
        Task<CommentFlagReadDTO> GetCommentFlag(string userId, int commentId);
        Task<IEnumerable<CommentReadDTO>> GetOwnCommentFlags(string userId);
        Task<CommentFlagReadDTO> FlagAComment(string userId, int commentId, CommentFlagReason reason);
    }
}