using LostAndFound.Infrastructure.DTOs.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IItemService
    {
        Task UpdateItemStatus(Guid itemId);
        /*Task<ItemReadDTO> UpdateItemDetail(Guid itemId, ItemUpdateWriteDTO itemUpdateWriteDTO);*/
        Task DeleteItemAsync(Guid itemId);
        Task<ItemReadDTO> FindItemById(Guid itemId);
        Task<DTOs.Common.PaginatedResponse<ItemReadDTO>> QueryItemAsync(ItemQuery query);
    }
}
