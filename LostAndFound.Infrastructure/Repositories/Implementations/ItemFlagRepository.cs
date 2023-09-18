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
    public class ItemFlagRepository : GenericRepository<ItemFlag>, IItemFlagRepository
    {
        public ItemFlagRepository(LostAndFoundDbContext context) : base(context)
        {
        }


        public async Task<int> CountItemFlagAsync(int itemId)
        {
            var result = _context.ItemFlags.Where(
                itf => itf.ItemId == itemId
                       &&
                       itf.IsActive == true
            );
            return await Task.FromResult(result.Count());
        }

        public async Task<IEnumerable<Item>> FindItemFlagsByUserIdAsync(string userId)
        {
            IQueryable<ItemFlag> itemFlags = _context.ItemFlags.Where(
                itf => itf.UserId == userId && itf.IsActive == true
            );
            IQueryable<Item> items = itemFlags.Select(itf => itf.Item);
            
            return await Task.FromResult(items.ToList());
        }

        public async Task<ItemFlag> FindItemFlagAsync(int itemId, string userId)
        {
            return await _context.ItemFlags
                .Include(itf => itf.Item)
                .FirstOrDefaultAsync(itf => itf.ItemId == itemId && itf.UserId == userId);
        }
        
    }
}