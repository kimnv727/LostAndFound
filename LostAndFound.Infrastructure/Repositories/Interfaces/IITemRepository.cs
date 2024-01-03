﻿using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IItemRepository :
        IGetAllAsync<Item>,
        IDelete<Item>,
        IUpdate<Item>,
        IFindAsync<Item>,
        IAddAsync<Item>
    {
        
        public Task<Item> FindItemByIdAsync(int ItemId);
        public Task<Item> FindItemByNameAsync(string Name);
        public Task<IEnumerable<Item>> QueryItemAsync(ItemQueryWithStatus query, bool trackChanges = false);
        public Task<IEnumerable<Item>> QueryRecentlyReturnedItemAsync(ItemQueryWithStatus query, bool trackChanges = false);
        public Task<IEnumerable<Item>> QueryItemExcludePendingRejectedAsync(ItemQueryIgnoreStatusExcludePendingRejected query, bool trackChanges = false);
        public Task<IEnumerable<Item>> QueryItemExcludePendingRejectedWithFlagAsync(ItemQueryWithFlag query, bool trackChanges = false);
        public Task<IEnumerable<Item>> GetItemsWithClaimsForMember(string userId);
        public Task<IEnumerable<Item>> GetItemsByFloorNumberAsync(int floorNumber);
        public Task<IEnumerable<Item>> GetItemsSortByFloorNumberAsync();
        public Task<IEnumerable<Item>> GetAllItemsWithClaimsForManager();
        public Task<Item> GetAllClaimsOfAnItemForFounder(string userId, int itemId);
        public Task<Item> GetAllClaimsOfAnItemForMember(string userId, int itemId);
        public Task<Item> GetAllClaimsOfAnItemForManager(int itemId);
        public Task UpdateItemRange(Item[] items);
        public Task<IEnumerable<Item>> GetAllActiveItems();
        public Task<IEnumerable<Item>> GetRecommendItemsByUserId(string userId);
        public Task<IEnumerable<Item>> GetItemsByLocationAndCategoryAsync(int locationId, int categoryId);
        public Task<IEnumerable<Item>> CountNewlyCreatedItem(int month, int year);
        public Task<IEnumerable<DTOs.Dashboard.Data>> GetItemCountsInDateRanges(int month, int year);
    }
}
