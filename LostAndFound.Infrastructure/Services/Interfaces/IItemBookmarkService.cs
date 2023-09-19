using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.ItemBookmark;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IItemBookmarkService
    {
        public Task<int> CountItemBookmarkAsync(int itemId);
        public Task<ItemBookmarkReadDTO> GetItemBookmark(string userId, int itemId);
        public Task<IEnumerable<ItemReadDTO>> GetOwnItemBookmarks(string userId);
        public Task<ItemBookmarkReadDTO> BookmarkAnItem(string userId, int itemId);
    }
}