using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(LostAndFoundDbContext context) : base(context) { }


        public Task<Item> FindItemByIdAsync(int ItemId)
        {
            return _context.Items
                .Include(i => i.Category)
                .Include(i => i.Location)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .FirstOrDefaultAsync(i => i.Id == ItemId);
        }

        public Task<Item> FindItemByNameAsync(string Name)
        {
            return _context.Items
                .Include(i => i.Category)
                .Include(i => i.Location)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .FirstOrDefaultAsync
                (i => i.Name.ToLower().Contains(Name.ToLower()));
        }

        public async Task<IEnumerable<Item>> QueryItemAsync(ItemQuery query, bool trackChanges = false)
        {
            //IQueryable<Item> items = _context.Items.Where(i => i.IsActive == true).AsSplitQuery();
            IQueryable<Item> items = _context.Items
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .AsSplitQuery();

            if (!trackChanges)
            {
                items = items.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                items = items.Where(i => i.Name.ToLower().Contains(query.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                items = items.Where(i => i.Description.ToLower().Contains(query.Description.ToLower()));
            }

            return await Task.FromResult(items.ToList());
        }

        public async Task<IEnumerable<Item>> QueryItemIgnoreStatusAsync(ItemQuery query, bool trackChanges = false)
        {
            IQueryable<Item> items = _context.Items
                                .Include(i => i.Category)
                                .Include(i => i.Location)
                                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                                .ThenInclude(im => im.Media)
                                .AsSplitQuery();

            if (!trackChanges)
            {
                items = items.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                items = items.Where(i => i.Name.ToLower().Contains(query.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                items = items.Where(i => i.Description.ToLower().Contains(query.Description.ToLower()));
            }

            return await Task.FromResult(items.ToList());
        }
    }
}
