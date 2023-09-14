using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.DTOs.PostBookmark;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IPostBookmarkService
    {
        public Task<int> CountPostBookmarkAsync(int postId);
        public Task<PostBookmarkReadDTO> GetPostBookmark(string userId, int postId);
        public Task<IEnumerable<PostReadDTO>> GetOwnPostBookmarkeds(string userId);
        public Task<PostBookmarkReadDTO> BookmarkAPost(string userId, int postId);
    }
}