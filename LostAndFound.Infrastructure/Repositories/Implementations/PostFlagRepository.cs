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
    public class PostFlagRepository : GenericRepository<PostFlag>, IPostFlagRepository
    {
        public PostFlagRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        /*public async Task<int> CountPostFlagAsync(int postId)
        {
            var result = _context.PostFlags.Where(pf => pf.PostId == postId && pf.IsActive == true);
            return await Task.FromResult(result.Count());
        }*/

        public async Task<IEnumerable<PostFlag>> CountPostFlagAsync(int postId)
        {
            var result = _context.PostFlags.Where(pf => pf.PostId == postId && pf.IsActive == true);
            return await Task.FromResult(result.ToList());
        }

        public async Task<IEnumerable<Post>> FindPostFlagsByUserIdAsync(string userId)
        {
            IQueryable<PostFlag> postFlags = _context.PostFlags.Where(pf => pf.UserId == userId && pf.IsActive == true);
            IQueryable<Post> posts = postFlags.Select(bf => bf.Post);

            return await Task.FromResult(posts.ToList());
        }
        
        public async Task<PostFlag> FindPostFlagAsync(int postId, string userId)
        {
            return await _context.PostFlags.Include(pf => pf.Post).FirstOrDefaultAsync(pf => pf.PostId == postId && pf.UserId == userId);
        }
    }
}