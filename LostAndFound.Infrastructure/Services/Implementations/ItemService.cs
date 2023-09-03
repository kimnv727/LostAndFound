using AutoMapper;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IITemRepository _itemRepository;

        public Task DeleteItemAsync(Guid itemId)
        {
            throw new NotImplementedException();
        }

        public Task<ItemReadDTO> FindItemById(Guid itemId)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedResponse<ItemReadDTO>> QueryItemAsync(ItemQuery query)
        {
            var items = await _itemRepository.QueryItemAsync(query);
            return PaginatedResponse<ItemReadDTO>.FromEnumerableWithMapping(items, query, _mapper);
        }

        public Task UpdateItemStatus(Guid itemId)
        {
            throw new NotImplementedException();
        }
    }
}
