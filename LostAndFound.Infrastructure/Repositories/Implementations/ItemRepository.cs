﻿using Firebase.Auth;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Extensions;
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
                .ThenInclude(l => l.Campus)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(i => i.ItemClaims)
                .Include(i => i.Cabinet)
                .ThenInclude(c => c.Storage)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(i => i.Id == ItemId);
        }

        public Task<Item> FindItemByNameAsync(string Name)
        {
            return _context.Items
                .Include(i => i.User)
                .ThenInclude(u => u.Campus)
                .Include(i => i.Category)
                .Include(i => i.Location)
                .ThenInclude(l => l.Campus)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(i => i.ItemClaims)
                .Include(i => i.Cabinet)
                .ThenInclude(c => c.Storage)
                .ThenInclude(s => s.User)
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
                .ThenInclude(l => l.Campus)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(i => i.ItemClaims)
                .Include(i => i.Cabinet)
                .ThenInclude(c => c.Storage)
                .ThenInclude(s => s.User)
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
                .ThenInclude(l => l.Campus)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(i => i.ItemClaims)
                .Include(i => i.Cabinet)
                .ThenInclude(c => c.Storage)
                .ThenInclude(s => s.User)
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
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
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
            /*if (query.LocationId > 0)
            {
                items = items.Where(i => i.LocationId == query.LocationId);
            }*/
            if (query.LocationId != null)
            {
                items = items.Where(i => query.LocationId.Contains(i.LocationId));
            }
            if (query.Floor >= 0)
            {
                items = items.Where(i => i.Location.Floor == query.Floor);
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
             }
             if (query.CategoryGroupId > 0)
             {
                 items = items.Where(i => i.Category.CategoryGroupId == query.CategoryGroupId);
             }*/
            if (query.CategoryGroupId != null)
            {
                items = items.Where(i => query.CategoryGroupId.Contains(i.Category.CategoryGroupId));
            }
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
            /*if (query.FoundDate > DateTime.MinValue)
            {
                items = items.Where(i => i.FoundDate == query.FoundDate).OrderBy(i => i.FoundDate);
            }*/
            if (!string.IsNullOrWhiteSpace(query.FoundDateFrom))
            {
                items = items.Where(i => i.FoundDate.CompareTo(query.FoundDateFrom) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(query.FoundDateTo))
            {
                items = items.Where(i => i.FoundDate.CompareTo(query.FoundDateTo) <= 0);
            }
            /*if (Enum.IsDefined(query.CampusLocation))
            {
                switch (query.CampusLocation)
                {

                    case ItemQueryWithStatus.CampusLocationQuery.ALL:
                        break;
                    case ItemQueryWithStatus.CampusLocationQuery.HO_CHI_MINH:
                        items = items.Where(i => i.Location.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case ItemQueryWithStatus.CampusLocationQuery.DA_NANG:
                        items = items.Where(i => i.Location.Campus.CampusLocation == CampusLocation.DA_NANG);
                        break;
                    case ItemQueryWithStatus.CampusLocationQuery.CAN_THO:
                        items = items.Where(i => i.Location.Campus.CampusLocation == CampusLocation.CAN_THO);
                        break;
                    case ItemQueryWithStatus.CampusLocationQuery.HA_NOI:
                        items = items.Where(i => i.Location.Campus.CampusLocation == CampusLocation.HA_NOI);
                        break;
                    default:
                        break;
                }
            }*/
            if (query.CampusId > 0)
            {
                items = items.Where(i => i.Location.CampusId == query.CampusId);
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

        public async Task<IEnumerable<Item>> QueryRecentlyReturnedItemAsync(string userId, ItemReturnedQuery query, bool trackChanges = false)
        {
            IQueryable<Item> items = _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
                            .Include(i => i.Receipts)
                            .ThenInclude(r => r.Media)
                            .Where(i => i.ItemStatus == ItemStatus.RETURNED && i.FoundUserId != userId
                            && i.Receipts.OrderByDescending(r => r.CreatedDate).FirstOrDefault(r => 
                            (r.ReceiptType == ReceiptType.RETURN_OUT_STORAGE || r.ReceiptType == ReceiptType.RETURN_USER_TO_USER)
                            && r.IsActive == true).CreatedDate.AddDays(7) >= DateTime.Now 
                            && i.Receipts.OrderByDescending(r => r.CreatedDate).FirstOrDefault(r =>
                            (r.ReceiptType == ReceiptType.RETURN_OUT_STORAGE || r.ReceiptType == ReceiptType.RETURN_USER_TO_USER)
                            && r.IsActive == true).ReceiverId != userId)
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
            /*if (query.LocationId > 0)
            {
                items = items.Where(i => i.LocationId == query.LocationId);
            }*/
            if (query.LocationId != null)
            {
                items = items.Where(i => query.LocationId.Contains(i.LocationId));
            }
            if (query.Floor >= 0)
            {
                items = items.Where(i => i.Location.Floor == query.Floor);
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
            }
            if (query.CategoryGroupId > 0)
            {
                items = items.Where(i => i.Category.CategoryGroupId == query.CategoryGroupId);
            }*/
            if (query.CategoryGroupId != null)
            {
                items = items.Where(i => query.CategoryGroupId.Contains(i.Category.CategoryGroupId));
            }
            if (query.CategoryId != null)
            {
                items = items.Where(i => query.CategoryId.Contains(i.CategoryId));
            }
            if (!string.IsNullOrWhiteSpace(query.FoundDateFrom))
            {
                items = items.Where(i => i.FoundDate.CompareTo(query.FoundDateFrom) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(query.FoundDateTo))
            {
                items = items.Where(i => i.FoundDate.CompareTo(query.FoundDateTo) <= 0);
            }
            if (query.CampusId > 0)
            {
                items = items.Where(i => i.Location.CampusId == query.CampusId);
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                items = items.OrderBy(query.OrderBy);
            }
            if (query.CreatedDate > DateTime.MinValue)
            {
                items = items.Where(i => i.CreatedDate == query.CreatedDate).OrderBy(i => i.CreatedDate);
            }
            if (query.ReturnedDateFrom > DateTime.MinValue)
            {
                items = items.Where(i => i.Receipts.OrderByDescending(r => r.CreatedDate).FirstOrDefault(r =>
                            (r.ReceiptType == ReceiptType.RETURN_OUT_STORAGE || r.ReceiptType == ReceiptType.RETURN_USER_TO_USER)
                            && r.IsActive == true).CreatedDate >= query.ReturnedDateFrom);
            }
            if (query.ReturnedDateTo > DateTime.MinValue)
            {
                items = items.Where(i => i.Receipts.OrderByDescending(r => r.CreatedDate).FirstOrDefault(r =>
                            (r.ReceiptType == ReceiptType.RETURN_OUT_STORAGE || r.ReceiptType == ReceiptType.RETURN_USER_TO_USER)
                            && r.IsActive == true).CreatedDate <= query.ReturnedDateTo);
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
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
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
            /*if (query.LocationId > 0)
            {
                items = items.Where(i => i.LocationId == query.LocationId);
            }*/
            if (query.LocationId != null)
            {
                items = items.Where(i => query.LocationId.Contains(i.LocationId));
            }
            if (query.Floor >= 0)
            {
                items = items.Where(i => i.Location.Floor == query.Floor);
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
            }
            if (query.CategoryGroupId > 0)
            {
                items = items.Where(i => i.Category.CategoryGroupId == query.CategoryGroupId);
            }*/
            if (query.CategoryGroupId != null)
            {
                items = items.Where(i => query.CategoryGroupId.Contains(i.Category.CategoryGroupId));
            }
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
            /*if (query.FoundDate > DateTime.MinValue)
            {
                items = items.Where(i => i.FoundDate == query.FoundDate).OrderBy(i => i.FoundDate);
            }*/
            if (!string.IsNullOrWhiteSpace(query.FoundDateFrom))
            {
                items = items.Where(i => i.FoundDate.CompareTo(query.FoundDateFrom) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(query.FoundDateTo))
            {
                items = items.Where(i => i.FoundDate.CompareTo(query.FoundDateTo) <= 0);
            }
            /*if (Enum.IsDefined(query.CampusLocation))
            {
                switch (query.CampusLocation)
                {

                    case ItemQueryIgnoreStatusExcludePendingRejected.CampusLocationQuery.ALL:
                        break;
                    case ItemQueryIgnoreStatusExcludePendingRejected.CampusLocationQuery.HO_CHI_MINH:
                        items = items.Where(i => i.Location.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case ItemQueryIgnoreStatusExcludePendingRejected.CampusLocationQuery.DA_NANG:
                        items = items.Where(i => i.Location.Campus.CampusLocation == CampusLocation.DA_NANG);
                        break;
                    case ItemQueryIgnoreStatusExcludePendingRejected.CampusLocationQuery.CAN_THO:
                        items = items.Where(i => i.Location.Campus.CampusLocation == CampusLocation.CAN_THO);
                        break;
                    case ItemQueryIgnoreStatusExcludePendingRejected.CampusLocationQuery.HA_NOI:
                        items = items.Where(i => i.Location.Campus.CampusLocation == CampusLocation.HA_NOI);
                        break;
                    default:
                        break;
                }
            }*/
            if (query.CampusId > 0)
            {
                items = items.Where(i => i.Location.CampusId == query.CampusId);
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
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.ItemFlags)
                            .Where(i => i.ItemStatus != ItemStatus.PENDING && i.ItemStatus != ItemStatus.REJECTED 
                            && i.ItemFlags.Where(ifg => ifg.IsActive == true).Count() > 0)
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
            /*if (query.LocationId > 0)
            {
                items = items.Where(i => i.LocationId == query.LocationId);
            }*/
            if (query.LocationId != null)
            {
                items = items.Where(i => query.LocationId.Contains(i.LocationId));
            }
            if (query.Floor >= 0)
            {
                items = items.Where(i => i.Location.Floor == query.Floor);
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                items = items.Where(i => i.Name.ToLower().Contains(query.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                items = items.Where(i => i.Description.ToLower().Contains(query.Description.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.FoundDateFrom))
            {
                items = items.Where(i => i.FoundDate.CompareTo(query.FoundDateFrom) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(query.FoundDateTo))
            {
                items = items.Where(i => i.FoundDate.CompareTo(query.FoundDateTo) <= 0);
            }
            /*if (query.CategoryId > 0)
            {
                items = items.Where(i => i.CategoryId == query.CategoryId);
            }
            if (query.CategoryGroupId > 0)
            {
                items = items.Where(i => i.Category.CategoryGroupId == query.CategoryGroupId);
            }*/
            if (query.CategoryGroupId != null)
            {
                items = items.Where(i => query.CategoryGroupId.Contains(i.Category.CategoryGroupId));
            }
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
            /*if (query.FoundDate > DateTime.MinValue)
            {
                items = items.Where(i => i.FoundDate == query.FoundDate).OrderBy(i => i.FoundDate);
            }*/
            if (!string.IsNullOrWhiteSpace(query.FoundDateFrom))
            {
                items = items.Where(i => i.FoundDate.CompareTo(query.FoundDateFrom) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(query.FoundDateTo))
            {
                items = items.Where(i => i.FoundDate.CompareTo(query.FoundDateTo) <= 0);
            }
            if (query.CampusId > 0)
            {
                items = items.Where(i => i.Location.CampusId == query.CampusId);
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
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .AsSplitQuery();

            items = items.Where(i => i.ItemClaims.Any(ic => ic.IsActive == true));

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
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .AsSplitQuery();

            
            items = items.Where(i => i.ItemClaims.Any(ic => ic.UserId.Equals(userId) && ic.IsActive == true));

            //Sort by claim date desceding 
            items = items.OrderByDescending(i => i.ItemClaims.Max(ic => ic.ClaimDate));

            return await Task.FromResult(items.ToList());
        }

        public async Task<IEnumerable<Item>> GetAllActiveItemsNotInStorageOfMember(string userId)
        {
            var items = _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .AsSplitQuery();


            items = items.Where(i => i.ItemStatus == ItemStatus.ACTIVE && i.FoundUserId == userId && i.IsInStorage == false);

            //Sort by claim date desceding 
            items = items.OrderByDescending(i => i.CreatedDate);

            return await Task.FromResult(items.ToList());
        }

        public async Task<IEnumerable<Item>> GetRecommendItemsByUserId(string userId)
        {
            var posts = _context.Posts
                .Include(p => p.Categories)
                .Include(p => p.Locations)
                .Where(p => p.PostUserId == userId && p.PostStatus != PostStatus.CLOSED
                && p.PostStatus != PostStatus.DELETED)
                .Take(5)
                .ToList();

            if(posts.Count() > 0)
            {
                var categories = posts.SelectMany(p => p.Categories)
                                      .GroupBy(c => c.Id)
                                      .Select(group => group.First())
                                      .Select(c => c.Id)
                                      .ToList();
                var locations = posts.SelectMany(p => p.Locations)
                                      .GroupBy(l => l.Id)
                                      .Select(group => group.First())
                                      .Select(l => l.Id)
                                      .ToList();

                if(categories.Count() > 0 && locations.Count() > 0)
                {
                    var items = _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .Where(i => i.ItemStatus == ItemStatus.ACTIVE 
                            && categories.Contains(i.CategoryId) && locations.Contains(i.LocationId) && i.FoundUserId != userId)
                            .Take(12)
                            .AsSplitQuery();

                    return await Task.FromResult(items.ToList());
                }
                else if (categories.Count() > 0)
                {
                    var items = _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .Where(i => i.ItemStatus == ItemStatus.ACTIVE && categories.Contains(i.CategoryId) && i.FoundUserId != userId)
                            .Take(12)
                            .AsSplitQuery();

                    return await Task.FromResult(items.ToList());
                }
                else if (locations.Count() > 0)
                {
                    var items = _context.Items
                            .Include(i => i.User)
                            .ThenInclude(u => u.Campus)
                            .Include(i => i.Category)
                            .Include(i => i.Location)
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.ItemClaims)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
                            .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                            .ThenInclude(im => im.Media)
                            .Where(i => i.ItemStatus == ItemStatus.ACTIVE && locations.Contains(i.LocationId) && i.FoundUserId != userId)
                            .Take(12)
                            .AsSplitQuery();

                    return await Task.FromResult(items.ToList());
                }
            }

            return null;
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
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
                            .Include(i => i.ItemClaims.Where(ic => ic.IsActive == true))
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
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
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
                            .ThenInclude(l => l.Campus)
                            .Include(i => i.Cabinet)
                            .ThenInclude(c => c.Storage)
                            .ThenInclude(s => s.User)
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
                .Where(i => i.ItemStatus == ItemStatus.ACTIVE
                //Also get those Item with OnHold status
                || i.ItemStatus == ItemStatus.ONHOLD);

            return await Task.FromResult(items.ToList());
        }

        public async Task<IEnumerable<Item>> GetItemsByLocationAndCategoryAsync(int locationId, int categoryId)
        {
            var items = _context.Items
                .Include(i => i.User)
                .ThenInclude(u => u.Campus)
                .Include(i => i.Category)
                .Include(i => i.Location)
                .ThenInclude(l => l.Campus)
                .Include(i => i.Cabinet)
                .ThenInclude(c => c.Storage)
                .ThenInclude(s => s.User)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Where(i => i.ItemStatus == ItemStatus.ACTIVE && i.LocationId == locationId && i.CategoryId == categoryId)
                .AsSplitQuery();

            items = items
                .OrderBy(i => i.CreatedDate);

            items = items.AsNoTracking();

            return await Task.FromResult(items.ToList());
        }

        public async Task<IEnumerable<Item>> CountNewlyCreatedItem(int month, int year)
        {
            var result = _context.Items.Where(i => i.CreatedDate.Month == month && i.CreatedDate.Year == year 
            && i.ItemStatus != ItemStatus.REJECTED && i.ItemStatus != ItemStatus.DELETED && i.ItemStatus != ItemStatus.PENDING);
            return await Task.FromResult(result.ToList());
        }

        public async Task<IEnumerable<DTOs.Dashboard.Data>> GetItemCountsInDateRanges(int month, int year)
        {
            //get number of days
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var result = new List<DTOs.Dashboard.Data>();

            for (int j = 0; j <= 8; j++)
            {
                var data = new DTOs.Dashboard.Data();
                data.x = (j * 3 + 3).ToString() + "/" + month.ToString() + "/" + year.ToString();
                data.y = _context.Items.Where(i => (i.CreatedDate.Day >= j * 3 + 1 && i.CreatedDate.Day <= j * 3 + 3)
                && i.CreatedDate.Month == month && i.CreatedDate.Year == year
                && i.ItemStatus != ItemStatus.REJECTED && i.ItemStatus != ItemStatus.DELETED && i.ItemStatus != ItemStatus.PENDING).Count();

                result.Add(data);
            }

            var lastData = new DTOs.Dashboard.Data();
            lastData.x = (daysInMonth).ToString() + "/" + month.ToString() + "/" + year.ToString();
            lastData.y = _context.Items.Where(i => (i.CreatedDate.Day >= 28 && i.CreatedDate.Day <= daysInMonth)
                && i.CreatedDate.Month == month && i.CreatedDate.Year == year
                && i.ItemStatus != ItemStatus.REJECTED && i.ItemStatus != ItemStatus.DELETED && i.ItemStatus != ItemStatus.PENDING).Count();
            result.Add(lastData);

            return result;
        }
    }


}
