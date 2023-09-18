using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IItemBookmarkRepository :
        IGetAllAsync<ItemBookmark>,
        IAddAsync<ItemBookmark>,
        IUpdate<ItemBookmark>,
        IDelete<ItemBookmark>
    {
        public Task<int> CountItemBookmarkAsync(int itemId);
        public Task<IEnumerable<Item>> FindItemBookmarksByUserIdAsync(string userId);
        public Task<ItemBookmark> FindItemBookmarkAsync(int itemId, string userId);
    }
}