using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.ItemFlag;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class ItemFlagService : IItemFlagService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IItemFlagRepository _itemFlagRepository;

        public ItemFlagService(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IItemRepository itemRepository, IItemFlagRepository itemFlagRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _itemFlagRepository = itemFlagRepository;
        }

        public async Task<int> CountItemFlagAsync(int itemId)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }

            var result = await _itemFlagRepository.CountItemFlagAsync(itemId);
            return result;
        }

        public async Task<ItemFlagReadDTO> GetItemFlag(string userId, int itemId)
        {
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }

            var itemFlag = await _itemFlagRepository.FindItemFlagAsync(itemId, userId);
            if (itemFlag == null)
            {
                throw new EntityNotFoundException<ItemFlag>();
            }

            return _mapper.Map <ItemFlagReadDTO>(itemFlag);
        }

        public async Task<IEnumerable<ItemFlagReadDTO>> GetOwnItemFlags(string userId)
        {
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }

            var result = await _itemFlagRepository.FindItemFlagsByUserIdAsync(userId);
            return _mapper.Map<List<ItemFlagReadDTO>>(result.ToList());
        }

        public async Task<ItemFlagReadDTO> FlagAnItem(string userId, int itemId, ItemFlagReason reason)
        {
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }

            var itemFlag = await _itemFlagRepository.FindItemFlagAsync(itemId, userId);
            if (itemFlag == null)
            {
                ItemFlag itf = new ItemFlag()
                {
                    ItemId = itemId,
                    UserId = userId,
                    ItemFlagReason = reason,
                    IsActive = true
                };
                await _itemFlagRepository.AddAsync(itf);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                switch (itemFlag.IsActive)
                {
                    case true:
                        itemFlag.IsActive = false;
                        break;
                    case false :
                    {
                        itemFlag.IsActive = true;
                        itemFlag.ItemFlagReason = reason;
                        break;
                    }
                }
                await _unitOfWork.CommitAsync();
            }

            return _mapper.Map<ItemFlagReadDTO>(itemFlag);
        }
    }
}