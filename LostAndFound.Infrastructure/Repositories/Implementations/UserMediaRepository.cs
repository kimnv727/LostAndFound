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
    public class UserMediaRepository : GenericRepository<UserMedia>, IUserMediaRepository
    {
        public UserMediaRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<UserMedia> FindUserMediaWithOnlyAvatarAsync(string userId)
        {
            return await _context.UserMedias
                .Where(um => um.Media.IsActive == true && um.Media.DeletedDate == null && um.MediaType == Core.Enums.UserMediaType.AVATAR)
                .Include(um => um.User)
                .Include(um => um.Media)
                .FirstOrDefaultAsync(um => um.UserId == userId);
                    
        }

        public async Task<ICollection<UserMedia>> FindUserMediaWithMediasExceptAvatarAsync(string userId)
        {
            return await _context.UserMedias
                .Where(um => um.Media.IsActive == true && um.Media.DeletedDate == null && um.MediaType != Core.Enums.UserMediaType.AVATAR)
                .Include(um => um.User)
                .Include(um => um.Media)
                .Where(um => um.UserId == userId)
                .ToListAsync();

        }

        public async Task<ICollection<UserMedia>> FindUserMediaWithMediasAsync(string userId)
        {
            return await _context.UserMedias
                .Where(um => um.Media.IsActive == true && um.Media.DeletedDate == null)
                .Include(um => um.User)
                .Include(um => um.Media)
                .Where(um => um.UserId == userId)
                .ToListAsync();
        }
    }
}
