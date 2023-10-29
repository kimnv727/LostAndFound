using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(LostAndFoundDbContext context) : base(context) { }


        public Task<Item> FindItemByIdAsync(int ItemId)
        {
            return _context.Items
                .Include(i => i.User)
                .Include(i => i.Category)
                .Include(i => i.Location)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .FirstOrDefaultAsync(i => i.Id == ItemId);
        }

        public Task<Item> FindItemByNameAsync(string Name)
        {
            return _context.Items
                .Include(i => i.User)
                .Include(i => i.Category)
                .Include(i => i.Location)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .FirstOrDefaultAsync
                (i => i.Name.ToLower().Contains(Name.ToLower()));
        }

        public async Task<IEnumerable<Item>> QueryItemAsync(ItemQueryWithStatus query, bool trackChanges = false)
        {
            IQueryable<Item> items = _context.Items
                            .Include(i => i.User)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .Include(i=> i.ItemClaims)
                            .AsSplitQuery();

            if (!trackChanges)
            {
                items = items.AsNoTracking();
            }

            if (query.Id > 0)
            {
                items = items.Where(i => i.Id == query.Id);
            }
            if (!string.IsNullOrWhiteSpace(query.FoundUserId))
            {
                items = items.Where(i => i.FoundUserId.ToLower().Contains(query.FoundUserId.ToLower()));
            }
            if (query.LocationId > 0)
            {
                items = items.Where(i => i.LocationId == query.LocationId);
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                items = items.Where(i => i.Name.ToLower().Contains(query.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                items = items.Where(i => i.Description.ToLower().Contains(query.Description.ToLower()));
            }
            if (query.CategoryId > 0)
            {
                items = items.Where(i => i.CategoryId == query.CategoryId);
            }
            if (Enum.IsDefined(query.ItemStatus))
            {
                switch (query.ItemStatus)
                {
                    
                    case ItemQueryWithStatus.ItemStatusQuery.ALL:
                        break;
                    case ItemQueryWithStatus.ItemStatusQuery.PENDING:
                        items = items.Where(i => i.ItemStatus == ItemStatus.PENDING);
                        break;
                    case ItemQueryWithStatus.ItemStatusQuery.ACTIVE:
                        items = items.Where(i => i.ItemStatus == ItemStatus.ACTIVE);
                        break;
                    case ItemQueryWithStatus.ItemStatusQuery.RETURNED:
                        items = items.Where(i => i.ItemStatus == ItemStatus.RETURNED);
                        break;
                    case ItemQueryWithStatus.ItemStatusQuery.CLOSED:
                        items = items.Where(i => i.ItemStatus == ItemStatus.CLOSED);
                        break;
                    case ItemQueryWithStatus.ItemStatusQuery.REJECTED:
                        items = items.Where(i => i.ItemStatus == ItemStatus.REJECTED);
                        break;
                    default:
                        break;
                }
            }
            else
            {


            }
            if (query.FoundDate > DateTime.MinValue)
            {
                items = items.Where(i => i.FoundDate == query.FoundDate).OrderBy(i => i.FoundDate);
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                items = items.OrderBy(query.OrderBy);
            }
            if (query.CreatedDate > DateTime.MinValue)
            {
                items = items.Where(i => i.CreatedDate == query.CreatedDate).OrderBy(i => i.CreatedDate);
            }


            return await Task.FromResult(items.ToList());
        }
    }
}
