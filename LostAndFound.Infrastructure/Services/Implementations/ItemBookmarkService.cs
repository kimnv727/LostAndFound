using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.ItemBookmark;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class ItemBookmarkService : IItemBookmarkService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IItemBookmarkRepository _itemBookmarkRepository;

        public ItemBookmarkService(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IItemRepository itemRepository, IItemBookmarkRepository itemBookmarkRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _itemBookmarkRepository = itemBookmarkRepository;
        }

        public async Task<int> CountItemBookmarkAsync(int itemId)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }

            var result = await _itemBookmarkRepository.CountItemBookmarkAsync(itemId);
            return result;
        }

        public async Task<ItemBookmarkReadDTO> GetItemBookmark(string userId, int itemId)
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

            var itemFlag = await _itemBookmarkRepository.FindItemBookmarkAsync(itemId, userId);
            if (itemFlag == null)
            {
                throw new EntityNotFoundException<ItemBookmark>();
            }

            return _mapper.Map <ItemBookmarkReadDTO>(itemFlag);
        }

        public async Task<IEnumerable<ItemReadDTO>> GetOwnItemBookmarks(string userId)
        {
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }

            var result = await _itemBookmarkRepository.FindItemBookmarksByUserIdAsync(userId);
            return _mapper.Map<List<ItemReadDTO>>(result.ToList());
        }

        public async Task<ItemBookmarkReadDTO> BookmarkAnItem(string userId, int itemId)
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

            var ib = await _itemBookmarkRepository.FindItemBookmarkAsync(itemId, userId);
            if (ib == null)
            {
                ib = new ItemBookmark()
                {
                    ItemId = itemId,
                    UserId = userId,
                    IsActive = true
                };
                await _itemBookmarkRepository.AddAsync(ib);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                switch (ib.IsActive)
                {
                    case true:
                        ib.IsActive = false;
                        break;
                    case false :
                    {
                        ib.IsActive = true;
                        break;
                    }
                }
                await _unitOfWork.CommitAsync();
            }

            return _mapper.Map<ItemBookmarkReadDTO>(ib);
        }
    }
}