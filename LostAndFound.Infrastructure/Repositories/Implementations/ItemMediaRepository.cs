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
    public class ItemMediaRepository : GenericRepository<ItemMedia>, IItemMediaRepository
    {
        public ItemMediaRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ItemMedia>> FindItemMediaIncludeMediaAsync(int itemId)
        {
            return await _context.ItemMedias
                .Where(um => um.Media.IsActive == true && um.Media.DeletedDate == null)
                .Include(um => um.Item)
                .Include(um => um.Media)
                .Where(um => um.ItemId == itemId)
                .ToListAsync();
                    
        }

    }
}
