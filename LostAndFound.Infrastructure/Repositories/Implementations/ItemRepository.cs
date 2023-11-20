using Firebase.Auth;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.ItemClaim;
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
                .ThenInclude(u => u.Campus)
                .Include(i => i.Category)
                .Include(i => i.Location)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(i => i.ItemClaims)
                .FirstOrDefaultAsync(i => i.Id == ItemId);
        }

        public Task<Item> FindItemByNameAsync(string Name)
        {
            return _context.Items
                .Include(i => i.User)
                .ThenInclude(u => u.Campus)
                .Include(i => i.Category)
                .Include(i => i.Location)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(i => i.ItemClaims)
                .FirstOrDefaultAsync
                (i => i.Name.ToLower().Contains(Name.ToLower()));
        }

        public async Task<IEnumerable<Item>> GetItemsByFloorNumberAsync(int floorNumber)
        {
            var items = _context.Items
                .Include(i => i.User)
                .ThenInclude(u => u.Campus)
                .Include(i => i.Category)
                .Include(i => i.Location)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(i => i.ItemClaims)
                .AsSplitQuery();

            items = items.Where(i => i.Location.Floor == floorNumber)
                .OrderBy(i => i.Location.Floor);

            items = items.AsNoTracking();

            return await Task.FromResult(items.ToList());
        }

        public async Task<IEnumerable<Item>> GetItemsSortByFloorNumberAsync()
        {
            var items = _context.Items
                .Include(i => i.User)
                .ThenInclude(u => u.Campus)
                .Include(i => i.Category)
                .Include(i => i.Location)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(i => i.ItemClaims)
                .AsSplitQuery();

            items = items
                .OrderBy(i => i.Location.Floor);

            items = items.AsNoTracking();

            return await Task.FromResult(items.ToList());
        }

        public async Task<IEnumerable<Item>> QueryItemAsync(ItemQueryWithStatus query, bool trackChanges = false)
        {
            IQueryable<Item> items = _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .Include(i => i.ItemClaims)
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
            /*if (query.CategoryId > 0)
            {
                items = items.Where(i => i.CategoryId == query.CategoryId);
            }*/
            if (query.CategoryId != null)
            {
                items = items.Where(i => query.CategoryId.Contains(i.CategoryId));
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

        public async Task<IEnumerable<Item>> QueryItemExcludePendingRejectedAsync(ItemQueryIgnoreStatusExcludePendingRejected query, bool trackChanges = false)
        {
            IQueryable<Item> items = _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .Include(i => i.ItemClaims)
                            .Where(i => i.ItemStatus != ItemStatus.PENDING && i.ItemStatus != ItemStatus.REJECTED)
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
            /*if (query.CategoryId > 0)
            {
                items = items.Where(i => i.CategoryId == query.CategoryId);
            }*/
            if (query.CategoryId != null)
            {
                items = items.Where(i => query.CategoryId.Contains(i.CategoryId));
            }
            if (Enum.IsDefined(query.ItemStatus))
            {
                switch (query.ItemStatus)
                {

                    case ItemQueryIgnoreStatusExcludePendingRejected.ItemStatusQuery.ACTIVE:
                        items = items.Where(i => i.ItemStatus == ItemStatus.ACTIVE);
                        break;
                    case ItemQueryIgnoreStatusExcludePendingRejected.ItemStatusQuery.RETURNED:
                        items = items.Where(i => i.ItemStatus == ItemStatus.RETURNED);
                        break;
                    case ItemQueryIgnoreStatusExcludePendingRejected.ItemStatusQuery.CLOSED:
                        items = items.Where(i => i.ItemStatus == ItemStatus.CLOSED);
                        break;
                }
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

        public async Task<IEnumerable<Item>> QueryItemExcludePendingRejectedWithFlagAsync(ItemQueryWithFlag query, bool trackChanges = false)
        {
            IQueryable<Item> items = _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.ItemFlags)
                            .Where(i => i.ItemStatus != ItemStatus.PENDING && i.ItemStatus != ItemStatus.REJECTED && i.ItemFlags.Count() > 0)
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
            /*if (query.CategoryId > 0)
            {
                items = items.Where(i => i.CategoryId == query.CategoryId);
            }*/
            if (query.CategoryId != null)
            {
                items = items.Where(i => query.CategoryId.Contains(i.CategoryId));
            }
            if (Enum.IsDefined(query.ItemStatus))
            {
                switch (query.ItemStatus)
                {

                    case ItemQueryWithFlag.ItemStatusQuery.ACTIVE:
                        items = items.Where(i => i.ItemStatus == ItemStatus.ACTIVE);
                        break;
                    case ItemQueryWithFlag.ItemStatusQuery.RETURNED:
                        items = items.Where(i => i.ItemStatus == ItemStatus.RETURNED);
                        break;
                    case ItemQueryWithFlag.ItemStatusQuery.CLOSED:
                        items = items.Where(i => i.ItemStatus == ItemStatus.CLOSED);
                        break;
                }
            }
            if (query.FlagCount > 0)
            {
                items = items.Where(p => p.ItemFlags.Count() >= query.FlagCount);
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

        //For managers, Get Items with claims from all users
        //Returns a list of items that have been claimed and their claim objects
        public async Task<IEnumerable<Item>> GetAllItemsWithClaimsForManager()
        {
            var items = _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .AsSplitQuery();

            items = items.Where(i => i.ItemClaims.Any(ic => ic.ClaimStatus == true));

            //Sort by claim date desceding 
            items = items.OrderByDescending(i => i.ItemClaims.Max(ic => ic.ClaimDate));

            return await Task.FromResult(items.ToList());
        }

        //For member, get claims matching userId
        //Returns a list of items that matches userId
        public async Task<IEnumerable<Item>> GetItemsWithClaimsForMember(string userId)
        {
            var items = _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .AsSplitQuery();

            
            items = items.Where(i => i.ItemClaims.Any(ic => ic.UserId.Equals(userId) && ic.ClaimStatus == true));

            //Sort by claim date desceding 
            items = items.OrderByDescending(i => i.ItemClaims.Max(ic => ic.ClaimDate));

            return await Task.FromResult(items.ToList());
        }

        //For item founder, returns an item and all its claims
        public async Task<Item> GetAllClaimsOfAnItemForFounder(string userId, int itemId)
        {
            //Item founder can get all claims of the item they created
            var item = await _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .Include(i => i.ItemClaims.Where(ic => ic.ClaimStatus == true))
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .FirstOrDefaultAsync(i => i.Id == itemId && i.FoundUserId == userId);

            return await Task.FromResult(item);
        }

        //For member, returns an item and a claim matching userId
        public async Task<Item> GetAllClaimsOfAnItemForMember(string userId, int itemId)
        {
            //Member can only get their own claim
            var item = await _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .Include(i => i.ItemClaims.Where(ic => ic.UserId == userId))
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .FirstOrDefaultAsync(i => i.Id == itemId);

            
            return await Task.FromResult(item);
        }

        //Function for managers, get all claims (of an Item == itemId), from all users
        public async Task<Item> GetAllClaimsOfAnItemForManager(int itemId)
        {
            var item = await _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .FirstOrDefaultAsync(i => i.Id == itemId);

            return await Task.FromResult(item);
        }

        public async Task UpdateItemRange(Item[] items)
        {
            _context.Items.UpdateRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Item>> GetAllActiveItems()
        {
            var items = _context.Items
                .Where(i => i.ItemStatus == ItemStatus.ACTIVE);

            return await Task.FromResult(items.ToList());
        }

        public async Task<IEnumerable<Item>> GetItemsByLocationAndCategoryAsync(int locationId, int categoryId)
        {
            var items = _context.Items
                .Include(i => i.User)
                .ThenInclude(u => u.Campus)
                .Include(i => i.Category)
                .Include(i => i.Location)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Where(i => i.ItemStatus == ItemStatus.ACTIVE && i.LocationId == locationId && i.CategoryId == categoryId)
                .AsSplitQuery();

            items = items
                .OrderBy(i => i.CreatedDate);

            items = items.AsNoTracking();

            return await Task.FromResult(items.ToList());
        }
    }


}
