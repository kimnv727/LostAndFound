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
    public class ItemBookmarkRepository : GenericRepository<ItemBookmark>, IItemBookmarkRepository
    {
        public ItemBookmarkRepository(LostAndFoundDbContext context) : base(context)
        {
        }


        public async Task<int> CountItemBookmarkAsync(int itemId)
        {
            var result = _context.ItemBookMarks.Where(
                itf => itf.ItemId == itemId
                       &&
                       itf.IsActive == true
            );
            return await Task.FromResult(result.Count());
        }

        public async Task<IEnumerable<Item>> FindItemBookmarksByUserIdAsync(string userId)
        {
            var itemBookmarksId = _context.ItemBookMarks.Where(ib => ib.UserId == userId && ib.IsActive == true).Select(ib => ib.ItemId).ToList();
            var items = _context.Items
                .Where(i => itemBookmarksId.Contains(i.Id) && i.ItemStatus != Core.Enums.ItemStatus.DELETED)
                .Include(i => i.Category)
                .Include(i => i.Location)
                .Include(i => i.ItemClaims.Where(ic => ic.UserId == userId))
                .Include(i => i.User)
                .ThenInclude(i => i.Campus)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media);

            return await Task.FromResult(items.ToList());
        }

        public async Task<ItemBookmark> FindItemBookmarkAsync(int itemId, string userId)
        {
            return await _context.ItemBookMarks
                .Include(itf => itf.Item)
                .FirstOrDefaultAsync(itf => itf.ItemId == itemId && itf.UserId == userId);
        }
        
    }
}