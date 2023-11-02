using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Firebase.Auth.Repository;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Exceptions.ItemClaim;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.ItemClaim;
using LostAndFound.Infrastructure.Repositories.Implementations;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class ItemClaimService : IItemClaimService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemClaimRepository _itemClaimRepository;
        private readonly IItemRepository _itemRepository;
        private readonly Repositories.Interfaces.IUserRepository _userRepository;

        public ItemClaimService(IMapper mapper, IUnitOfWork unitOfWork, IItemClaimRepository itemClaimRepository, IItemRepository itemRepository, Repositories.Interfaces.IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _itemClaimRepository = itemClaimRepository;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
        }

        public async Task<PaginatedResponse<ItemClaimReadDTO>> QueryItemClaimAsync(ItemClaimQuery query)
        {
            var claims = await _itemClaimRepository.QueryItemClaimsAsync(query);
            return PaginatedResponse<ItemClaimReadDTO>.FromEnumerableWithMapping(claims, query, _mapper);
        }

        public async Task<IEnumerable<ItemClaimReadDTO>> GetClaimsByItemIdAsync(int itemId)
        {
            var claims = await _itemClaimRepository.GetAllClaimsByItemIdAsync(itemId);
            return _mapper.Map<List<ItemClaimReadDTO>>(claims.ToList());
        }

        public async Task<IEnumerable<ItemClaimReadDTO>> GetClaimsByUserIdAsync(string userId)
        {
            var claims = await _itemClaimRepository.GetAllClaimsByUserIdAsync(userId);
            return _mapper.Map<List<ItemClaimReadDTO>>(claims.ToList());
        }

        public async Task ClaimAnItemAsync(int itemId, string userId)
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
            else if (item != null)
            {
                if(item.FoundUserId == userId)
                {
                    throw new CannotClaimOwnItemException();
                }
            }

            var check = await _itemClaimRepository.FindClaimByItemIdAndUserId(itemId, userId);
            //If Claim record exists & status == true ==> User already claimed it
            if (check != null && check.ClaimStatus == true)
            {
                throw new DuplicateItemClaimException();
            }
            //If Claim record exists & status == false ==> User has unclaimed it
            else if (check != null && check.ClaimStatus == false)
            {
                ItemClaimWriteDTO itemClaimWriteDTO = new ItemClaimWriteDTO();
                itemClaimWriteDTO.UserId = userId;
                itemClaimWriteDTO.ItemId = itemId;
                itemClaimWriteDTO.ClaimStatus = true;
                itemClaimWriteDTO.ClaimDate = DateTime.Now.ToVNTime();

                _mapper.Map(itemClaimWriteDTO, check);
                await _unitOfWork.CommitAsync();
            }//If Claim record doesn not exists then create new
            else if (check == null)
            {
                ItemClaimWriteDTO itemClaimWriteDTO = new ItemClaimWriteDTO();
                itemClaimWriteDTO.UserId = userId;
                itemClaimWriteDTO.ItemId = itemId;
                itemClaimWriteDTO.ClaimStatus = true;
                itemClaimWriteDTO.ClaimDate = DateTime.Now.ToVNTime();

                var claim = _mapper.Map<ItemClaim>(itemClaimWriteDTO);
                await _itemClaimRepository.AddAsync(claim);
                await _unitOfWork.CommitAsync();
            }

        }

        public async Task UnClaimAnItemAsync(int itemId, string userId)
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

            var claim = await _itemClaimRepository.FindClaimByItemIdAndUserId(itemId, userId);
            if (claim == null)
            {
                throw new NoSuchClaimException();
            }

            ItemClaimWriteDTO itemClaimWriteDTO = new ItemClaimWriteDTO();
            itemClaimWriteDTO.UserId = userId;
            itemClaimWriteDTO.ItemId = itemId;
            itemClaimWriteDTO.ClaimStatus = false;
            itemClaimWriteDTO.ClaimDate = DateTime.MinValue;

            _mapper.Map(itemClaimWriteDTO, claim);
            await _unitOfWork.CommitAsync();
        }

    }

}
