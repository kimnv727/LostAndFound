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

        Task<IEnumerable<Item>> QueryItemAsync(ItemQuery query, bool trackChanges = false);
        Task<IEnumerable<Item>> QueryItemIgnoreStatusAsync(ItemQuery query, bool trackChanges = false);
    }
}
