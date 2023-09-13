using Microsoft.EntityFrameworkCore;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class PostMediaRepository : GenericRepository<PostMedia>, IPostMediaRepository
    {
        public PostMediaRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PostMedia>> FindPostMediaIncludeMediaAsync(int postId)
        {
            return await _context.PostMedias
                .Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null)
                .Include(pm => pm.Post)
                .Include(pm => pm.Media)
                .Where(pm => pm.PostId == postId)
                .ToListAsync();   
        }

    }
}
