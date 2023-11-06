using LostAndFound.Core.Entities;
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
        
        Task<Item> FindItemByIdAsync(int ItemId);
        Task<Item> FindItemByNameAsync(string Name);
        Task<IEnumerable<Item>> QueryItemAsync(ItemQueryWithStatus query, bool trackChanges = false);
        public Task<IEnumerable<Item>> GetItemsWithClaimsForMember(string userId);
        public Task<IEnumerable<Item>> GetAllItemsWithClaimsForManager();
        public Task<Item> GetAllClaimsOfAnItemForFounder(string userId, int itemId);
        public Task<Item> GetAllClaimsOfAnItemForMember(string userId, int itemId);
        public Task<Item> GetAllClaimsOfAnItemForManager(int itemId);

    }
}
