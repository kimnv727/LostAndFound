using System;
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
using System.Collections;
using System.Collections.Generic;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.DTOs.ItemClaim;
using System.Security.Claims;
using LostAndFound.Core.Exceptions.ItemClaim;
using System.Linq;
using AutoMapper.Configuration.Annotations;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly IItemMediaService _itemMediaService;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryGroupRepository _categoryGroupRepository;
        private readonly ICabinetRepository _cabinetRepository;
        private readonly IItemClaimRepository _itemClaimRepository;

        public ItemService(IMapper mapper, IUnitOfWork unitOfWork, IItemRepository itemRepository, IUserRepository userRepository,
            ICategoryRepository categoryRepository, ICategoryGroupRepository categoryGroupRepository, IItemMediaService itemMediaService,
            ICabinetRepository cabinetRepository, IItemClaimRepository itemClaimRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _categoryGroupRepository = categoryGroupRepository;
            _itemMediaService = itemMediaService;
            _cabinetRepository = cabinetRepository;
            _itemClaimRepository = itemClaimRepository;
        }

        public async Task<PaginatedResponse<ItemReadDTO>> QueryItemAsync(ItemQueryWithStatus query)
        {
            var items = await _itemRepository.QueryItemAsync(query);
            return PaginatedResponse<ItemReadDTO>.FromEnumerableWithMapping(items, query, _mapper);
        }
        public async Task<PaginatedResponse<ItemReadDTO>> QueryItemIgnorePendingRejectedAsync(ItemQueryIgnoreStatusExcludePendingRejected query)
        {
            var items = await _itemRepository.QueryItemExcludePendingRejectedAsync(query);
            return PaginatedResponse<ItemReadDTO>.FromEnumerableWithMapping(items, query, _mapper);
        }

        public async Task<PaginatedResponse<ItemDetailWithFlagReadDTO>> QueryItemIgnorePendingRejectedWithFlagAsync(ItemQueryWithFlag query)
        {
            var items = await _itemRepository.QueryItemExcludePendingRejectedWithFlagAsync(query);

            var result = new List<ItemDetailWithFlagReadDTO>();

            foreach (var i in items)
            {
                var r = _mapper.Map<ItemDetailWithFlagReadDTO>(i);
                r.BlurryCount = i.ItemFlags.Where(p => p.ItemFlagReason == ItemFlagReason.BLURRY).Count();
                r.WrongCount = i.ItemFlags.Where(p => p.ItemFlagReason == ItemFlagReason.WRONG).Count();
                r.InapproriateCount = i.ItemFlags.Where(p => p.ItemFlagReason == ItemFlagReason.INAPPROPRIATE).Count();
                r.TotalCount = i.ItemFlags.Count();
                result.Add(r);
            }

            return PaginatedResponse<ItemDetailWithFlagReadDTO>.FromEnumerableWithMapping(result, query, _mapper);
        }

        public async Task UpdateItemStatusAsync(int itemId)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);

            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ItemReadDTO>> ListItemsSortByFloorNumberAsync()
        {
            var items = await _itemRepository.GetItemsSortByFloorNumberAsync();

            return _mapper.Map<List<ItemReadDTO>>(items);
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
            item.ItemStatus = ItemStatus.PENDING;
            item.FoundUserId = user.Id;
            item.FoundDate = DateTime.Now.ToVNTime();

            await _itemRepository.AddAsync(item);
            await _unitOfWork.CommitAsync();

            //Add Media
            await _itemMediaService.UploadItemMedias(userId, item.Id, itemWriteDTO.Medias);
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

        public async Task<ItemDetailReadDTO> FindItemByIdAsync(int itemId)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);

            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }
            return _mapper.Map<ItemDetailReadDTO>(item);
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

        public async Task<ItemReadDTO> UpdateItemStatus(int itemId, ItemStatus itemStatus)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }

            item.ItemStatus = itemStatus;
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ItemReadDTO>(item);
        }

        public async Task<ItemReadDTO> FindItemByNameAsync(string name)
        {
            var item = await _itemRepository.FindItemByNameAsync(name);

            if (item == null)
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

        public async Task<IEnumerable<ItemReadWithClaimStatusDTO>> GetClaimsForMember(string userId)
        {
            var items = await _itemRepository.GetItemsWithClaimsForMember(userId);
            return _mapper.Map<List<ItemReadWithClaimStatusDTO>>(items);
        }

        public async Task<IEnumerable<ItemReadWithClaimStatusDTO>> GetAllClaimsForManager()
        {
            var items = await _itemRepository.GetAllItemsWithClaimsForManager();
            return _mapper.Map<List<ItemReadWithClaimStatusDTO>>(items);
        }

        public async Task<ItemReadWithClaimStatusDTO> GetAnItemWithClaimsForFounder(string userId, int itemId)
        {
            var item = await _itemRepository.GetAllClaimsOfAnItemForFounder(userId, itemId);
            return _mapper.Map<ItemReadWithClaimStatusDTO>(item);
        }

        public async Task<ItemReadWithClaimStatusDTO> GetAnItemWithClaimsForMember(string userId, int itemId)
        {
            var item = await _itemRepository.GetAllClaimsOfAnItemForMember(userId, itemId);
            return _mapper.Map<ItemReadWithClaimStatusDTO>(item);
        }

        public async Task<ItemReadWithClaimStatusDTO> GetAnItemWithClaimsForManager(int itemId)
        {
            var item = await _itemRepository.GetAllClaimsOfAnItemForManager(itemId);
            return _mapper.Map<ItemReadWithClaimStatusDTO>(item);
        }

        public async Task<ItemReadDTO> UpdateItemCabinet(int itemId, int cabinetId)
        {
            //check Item
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
                throw new EntityWithIDNotFoundException<Item>(itemId);

            //check Cabinet
            var cabinet = await _cabinetRepository.FindCabinetByIdAsync(cabinetId);
            if (cabinet == null)
                throw new EntityWithIDNotFoundException<Cabinet>(cabinetId);

            item.CabinetId = cabinetId;
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ItemReadDTO>(item);
        }

        public async Task UpdateClaimStatusAsync(int itemId, string userId)
        {
            var claim = await _itemClaimRepository.FindClaimByItemIdAndUserId(itemId, userId);
            if (claim == null)
            {
                throw new NoSuchClaimException();
            }

            claim.ClaimStatus = !claim.ClaimStatus;
            await _unitOfWork.CommitAsync();
        }

        public async Task AcceptAClaimAsync(int itemId, string userId)
        {
            /*
            Accept a claim:
            + Change item status to RETURNED
            + Set all claims to status = false, except for the claimer
            + Set "get items with claims for member" to query for status=returned if member claimed it
            */

            //Check userId 
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }

            //Check if claim status = true
            var check = await _itemClaimRepository.FindClaimByItemIdAndUserId(itemId, userId);
            if(check == null)
            {
                throw new NoSuchClaimException();
            }
            if (check.ClaimStatus == false)
            {
                throw new CannotAcceptDisabledClaimException();
            }

            //Get item and change item status to RETURNED
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }
            //Change item status
            await UpdateItemStatus(itemId, ItemStatus.RETURNED);
            await _unitOfWork.CommitAsync();
            //Set only one claim to true and the rest to false

            //Set all claims of this item to status = false
            var claimsOfThisItem = await _itemClaimRepository.GetAllClaimsByItemIdAsync(itemId);
            foreach (var claim in claimsOfThisItem)
            {
                if (claim.UserId != userId)
                {
                    claim.ClaimStatus = false;
                    await _unitOfWork.CommitAsync();
                }
            }
        }

        public async Task DenyAClaimAsync(int itemId, string userId)
        {
            //Check userId & check if item exists
            var user = await _userRepository.FindUserByID(userId) ?? throw new EntityWithIDNotFoundException<User>(userId);
            var item = await _itemRepository.FindItemByIdAsync(itemId) ?? throw new EntityWithIDNotFoundException<Item>(itemId);
            //Set this claim to status = false
            var claim = await _itemClaimRepository.FindClaimByItemIdAndUserId(itemId, userId) ?? throw new EntityNotFoundException<ItemClaim>();
            claim.ClaimStatus = false;
            await _unitOfWork.CommitAsync();
        }
    }
}
