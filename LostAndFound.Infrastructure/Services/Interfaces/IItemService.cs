using LostAndFound.Infrastructure.DTOs.Item;
using System;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IItemService
    {
        Task UpdateItemStatusAsync(int itemId);
        /*Task<ItemReadDTO> UpdateItemDetail(int itemId, ItemUpdateWriteDTO itemUpdateWriteDTO);*/
        Task DeleteItemAsync(int itemId);
        Task<ItemDetailReadDTO> FindItemByIdAsync(int itemId);
        Task<ItemReadDTO> FindItemNameAsync(String name);
        Task<DTOs.Common.PaginatedResponse<ItemReadDTO>> QueryItemAsync(ItemQuery query);
        Task<DTOs.Common.PaginatedResponse<ItemReadDTO>> QueryItemIgnoreStatusAsync(ItemQuery query);
        Task<bool> CheckItemFounderAsync(int itemId, string userId);
        Task<ItemReadDTO> UpdateItemDetailsAsync(int itemId, ItemUpdateDTO itemUpdateDTO);
        public Task<ItemReadDTO> CreateItemAsync(string userId, ItemWriteDTO itemWriteDTO);
        //public Task<ItemReadDTO> CreateItemAsync(ItemValue itemValue, string categoryName, string userId, ItemWriteDTO itemWriteDTO);

    }
}
