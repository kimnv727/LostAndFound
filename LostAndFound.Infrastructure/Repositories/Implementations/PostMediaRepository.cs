using Microsoft.EntityFrameworkCore;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class PostMediaRepository : GenericRepository<PostMedia>, IPostMediaRepository
    {
        public PostMediaRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<PostMedia> FindPostMediaIncludeMediaAsync(int postId)
        {
            return await _context.PostMedias
                .Where(um => um.Media.IsActive == true && um.Media.DeletedDate == null)
                .Include(um => um.Post)
                .Include(um => um.Media)
                .FirstOrDefaultAsync(um => um.PostId == postId);
                    
        }

    }
}
