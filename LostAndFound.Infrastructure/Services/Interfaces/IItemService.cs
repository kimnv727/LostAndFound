using LostAndFound.Infrastructure.DTOs.Item;
using System;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IItemService
    {
        Task UpdateItemStatusAsync(int itemId);
        /*Task<ItemReadDTO> UpdateItemDetail(int itemId, ItemUpdateWriteDTO itemUpdateWriteDTO);*/
        Task DeleteItemAsync(int itemId);
        Task<ItemReadDTO> FindItemByIdAsync(int itemId);
        Task<ItemReadDTO> FindItemNameAsync(String name);
        Task<DTOs.Common.PaginatedResponse<ItemReadDTO>> QueryItemAsync(ItemQuery query);
        Task<DTOs.Common.PaginatedResponse<ItemReadDTO>> QueryItemIgnoreStatusAsync(ItemQuery query);
        Task<ItemReadDTO> UpdateItemDetailsAsync(int itemId, ItemWriteDTO itemWriteDTO);
        Task<bool> CheckItemFounderAsync(int itemId, string userId);
    }
}
