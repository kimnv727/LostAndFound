using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IPostBookmarkRepository :
        IGetAllAsync<PostBookmark>,
        IAddAsync<PostBookmark>,
        IUpdate<PostBookmark>,
        IDelete<PostBookmark>
    {
        Task<int> CountPostBookmarkAsync(int postId);
        Task<PostBookmark> FindPostBookmarkAsync(int postId, string userId);
        Task<IEnumerable<Post>> FindBookmarkPostsByUserIdAsync(string userId);
    }
}