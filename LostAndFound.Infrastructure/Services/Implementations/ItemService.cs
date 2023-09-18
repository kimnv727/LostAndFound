using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        public ItemService(IMapper mapper, IUnitOfWork unitOfWork, IItemRepository itemRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
        }

        public async Task<PaginatedResponse<ItemReadDTO>> QueryItemAsync(ItemQuery query)
        {
            var items = await _itemRepository.QueryItemAsync(query);
            return PaginatedResponse<ItemReadDTO>.FromEnumerableWithMapping(items, query, _mapper);
        }
        
        public async Task<PaginatedResponse<ItemReadDTO>> QueryItemIgnoreStatusAsync(ItemQuery query)
        {
            var items = await _itemRepository.QueryItemIgnoreStatusAsync(query);
            return PaginatedResponse<ItemReadDTO>.FromEnumerableWithMapping(items, query, _mapper);
        }

        public Task<ItemReadDTO> UpdateItemDetailsAsync(int itemId, ItemWriteDTO itemWriteDTO)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateItemStatusAsync(int itemId)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);

            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }

            /*if (item.IsActive == true)
            {
                _itemRepository.Delete(item);
            }
            else if (item.IsActive == false)
            {
                item.IsActive = true;
                item.DeletedDate = null;
                item.DeletedDate = null;
            }*/
            await _unitOfWork.CommitAsync();

        }

        public async Task<ItemReadDTO> CreateItemAsync(string userId, ItemWriteDTO itemWriteDTO)
        {
            //Check if user exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            
            var item = _mapper.Map<Item>(itemWriteDTO);
            item.FoundUserId = user.Id;
            item.ItemStatus = ItemStatus.PENDING;
            await _itemRepository.AddAsync(item);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ItemReadDTO>(item);

        }

        public async Task DeleteItemAsync(int itemId)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);

            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }

            _itemRepository.Delete(item);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ItemReadDTO> FindItemByIdAsync(int itemId)
        {
            var item = await _itemRepository.FindAsync(itemId);

            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }
            return _mapper.Map<ItemReadDTO>(item);
        }

        public async Task<ItemReadDTO> UpdateItemDetailsAsync(int itemId, ItemUpdateDTO itemUpdateDTO)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }

            _mapper.Map(itemUpdateDTO, item);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ItemReadDTO>(item);
        }

        public async Task<ItemReadDTO> FindItemNameAsync(string name)
        {
            var item = await _itemRepository.FindItemByNameAsync(name);

            if(item == null)
            {
                //Need EntityWithNameNotFoundException type
                throw new EntityNotFoundException("Entity with name {" + name + "} not found.");
            }

            return _mapper.Map<ItemReadDTO>(item);
        }

        public async Task<bool> CheckItemFounderAsync(int itemId, string userId)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);

            if (item == null)
                throw new EntityWithIDNotFoundException<Item>(itemId);

            return item.FoundUserId == userId ? true : false;
        }
    }
}
