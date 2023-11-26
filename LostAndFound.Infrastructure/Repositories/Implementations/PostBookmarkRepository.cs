using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class PostBookmarkRepository : GenericRepository<PostBookmark>, IPostBookmarkRepository
    {
        public PostBookmarkRepository(LostAndFoundDbContext context) : base(context)
        {
        }
        
        public async Task<int> CountPostBookmarkAsync(int postId)
        {
            var result = _context.PostBookmarks.Where(pb => pb.PostId == postId && pb.IsActive == true);
            return await Task.FromResult(result.Count());
        }

        public async Task<IEnumerable<Post>> FindBookmarkPostsByUserIdAsync(string userId)
        {
            var postBookmarksId = _context.PostBookmarks.Where(pb => pb.UserId == userId && pb.IsActive == true).Select(pb => pb.PostId).ToList();
            var posts = _context.Posts
                .Where(p => postBookmarksId.Contains(p.Id) && p.PostStatus != Core.Enums.PostStatus.DELETED)
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Include(p => p.User)
                .ThenInclude(p => p.Campus)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media);

            return await Task.FromResult(posts.ToList());
        }
        
        public async Task<PostBookmark> FindPostBookmarkAsync(int postId, string userId)
        {
            return await _context.PostBookmarks.Include(pb => pb.Post)
                .FirstOrDefaultAsync(pb => pb.PostId == postId && pb.UserId == userId);
        }
    }
}