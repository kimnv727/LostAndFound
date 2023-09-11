using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface ICommentRepository :
        IGetAllAsync<Comment>,
        IAddAsync<Comment>,
        IUpdate<Comment>,
        IDelete<Comment>
    {
        Task<Comment> FindCommentByIdAsync(int id);
        Task<Comment> FindCommentIgnoreStatusByIdAsync(int id);
        Task<Comment> FindCommentWithReplyByIdAsync(int id);
        Task<IEnumerable<Comment>> FindAllCommentsByPostIdAsync(int postId);
        Task<IEnumerable<Comment>> FindAllCommentsByUserIdAsync(string userId);
        //Task<IEnumerable<Comment>> FindAllCommentsReplyToCommentId(int commentId);
        Task<IEnumerable<Comment>> QueryCommentAsync(CommentQuery query, bool trackChanges = false);
        Task<IEnumerable<Comment>> QueryCommentIgnoreStatusAsync(CommentQuery query, bool trackChanges = false);

    }
}