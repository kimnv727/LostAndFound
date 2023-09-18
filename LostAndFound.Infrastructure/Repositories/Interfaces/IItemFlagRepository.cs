using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IItemFlagRepository :
        IGetAllAsync<ItemFlag>,
        IAddAsync<ItemFlag>,
        IUpdate<ItemFlag>,
        IDelete<ItemFlag>
    {
        public Task<int> CountItemFlagAsync(int itemId);
        public Task<IEnumerable<Item>> FindItemFlagsByUserIdAsync(string userId);
        public Task<ItemFlag> FindItemFlagAsync(int itemId, string userId);
    }
}