using Microsoft.EntityFrameworkCore;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class UserMediaRepository : GenericRepository<UserMedia>, IUserMediaRepository
    {
        public UserMediaRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<UserMedia> FindUserMediaIncludeMediaAsync(string userId)
        {
            return await _context.UserMedias
                .Where(um => um.Media.IsActive == true && um.Media.DeletedDate == null)
                .Include(um => um.User)
                .Include(um => um.Media)
                .FirstOrDefaultAsync(um => um.UserId == userId);
                    
        }

    }
}
