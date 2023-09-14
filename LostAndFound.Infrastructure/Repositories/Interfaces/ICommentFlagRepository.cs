using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface ICommentFlagRepository :
        IGetAllAsync<CommentFlag>,
        IAddAsync<CommentFlag>,
        IUpdate<CommentFlag>,
        IDelete<CommentFlag>
    {
        Task<int> CountCommentFlagAsync(int commentId);
        Task<CommentFlag> FindCommentFlagAsync(int commentId, string userId);
        Task<IEnumerable<Comment>> FindCommentFlagsByUserIdAsync(string userId);
    }
}