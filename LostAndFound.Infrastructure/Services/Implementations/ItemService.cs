﻿using System;
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
using F23.StringSimilarity;
using LostAndFound.Infrastructure.DTOs.Receipt;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Core.Exceptions.Item;
using Microsoft.AspNetCore.Http;

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
        private readonly IPostRepository _postRepository;
        private readonly IMediaService _mediaService;
        private readonly IReceiptRepository _receiptRepository;
        private readonly AwsCredentials _awsCredentials;
        private readonly IEmailSendingService _emailSendingService;

        public ItemService(IMapper mapper, IUnitOfWork unitOfWork, IItemRepository itemRepository, IUserRepository userRepository,
            ICategoryRepository categoryRepository, ICategoryGroupRepository categoryGroupRepository, IItemMediaService itemMediaService,
            ICabinetRepository cabinetRepository, IItemClaimRepository itemClaimRepository, IPostRepository postRepository,
            IMediaService mediaService, AwsCredentials awsCredentials, IReceiptRepository receiptRepository, IEmailSendingService emailSendingService)
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
            _postRepository = postRepository;
            _mediaService = mediaService;
            _awsCredentials = awsCredentials;
            _receiptRepository = receiptRepository;
            _emailSendingService = emailSendingService;
        }

        public async Task<PaginatedResponse<ItemReadDTO>> QueryItemAsync(ItemQueryWithStatus query)
        {
            var items = await _itemRepository.QueryItemAsync(query);
            return PaginatedResponse<ItemReadDTO>.FromEnumerableWithMapping(items, query, _mapper);
        }

        public async Task<PaginatedResponse<ItemReadDTO>> QueryRecentlyReturnedItemAsync(string userId, ItemReturnedQuery query)
        {
            var items = await _itemRepository.QueryRecentlyReturnedItemAsync(userId, query);
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
                r.BlurryCount = i.ItemFlags.Where(f => f.ItemFlagReason == ItemFlagReason.BLURRY && f.IsActive == true).Count();
                r.WrongCount = i.ItemFlags.Where(f => f.ItemFlagReason == ItemFlagReason.WRONG && f.IsActive == true).Count();
                r.InapproriateCount = i.ItemFlags.Where(f => f.ItemFlagReason == ItemFlagReason.INAPPROPRIATE && f.IsActive == true).Count();
                r.TotalCount = i.ItemFlags.Where(f => f.IsActive == true).Count();
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

        public async Task<IEnumerable<ItemReadDTO>> GetRecommendItemsByUserId(string userId)
        {
            var items = await _itemRepository.GetRecommendItemsByUserId(userId);

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
            if(user.RoleId == 3)
            {
                item.ItemStatus = ItemStatus.ACTIVE;
            }
            else
            {
                item.ItemStatus = ItemStatus.PENDING;
            }
            
            item.FoundUserId = user.Id;
            item.FoundDate = itemWriteDTO.FoundDate;
            if(user.RoleId == 3)
            {
                item.IsInStorage = true;
            }

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
            _emailSendingService.SendMailWhenItemBan(item.User.Email, item.Name);
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

        public async Task<ItemReadDTO> UpdateItemDetailsAsync(int itemId, ItemUpdateDTO itemUpdateDTO, string userId)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }

            if(itemUpdateDTO.CabinetId == 0)
            {
                itemUpdateDTO.CabinetId = null;
            }

            //get User
            var user = await _userRepository.FindUserByID(userId);
            if (item.ItemStatus == ItemStatus.REJECTED && user.RoleId != 3)
            {
                item.ItemStatus = ItemStatus.PENDING;
            }
            /*else
            {
                item.ItemStatus = ItemStatus.ACTIVE;
            }*/
            _mapper.Map(itemUpdateDTO, item);

            await _unitOfWork.CommitAsync();
            return _mapper.Map<ItemReadDTO>(item);
        }

        public async Task<ItemReadDTO> UpdateItemDetailsWithoutCabinetIdAsync(int itemId, ItemUpdateWithoutCabinetIdDTO itemUpdateDTO)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }

            if (item.ItemStatus == ItemStatus.REJECTED)
            {
                item.ItemStatus = ItemStatus.PENDING;
            }
            else
            {
                item.ItemStatus = ItemStatus.ACTIVE;
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

            if(itemStatus == ItemStatus.DELETED)
            {
                _emailSendingService.SendMailWhenItemBan(item.User.Email, item.Name);
            }
            if (itemStatus == ItemStatus.ACTIVE)
            {
                _emailSendingService.SendMailItemApprove(item.User.Email, item.Name);
            }
            if (itemStatus == ItemStatus.REJECTED)
            {
                _emailSendingService.SendMailItemReject(item.User.Email, item.Name);
            }

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

            claim.IsActive = !claim.IsActive;
            await _unitOfWork.CommitAsync();
        }

        public async Task AcceptAClaimAsync(int itemId, string receiverId)
        {
            /*
            + Change item status to RETURNED
            + Set all claims to status = false, except for the claim maker
            */

            //Check userId of claim maker
            var user = await _userRepository.FindUserByID(receiverId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(receiverId);
            }

            //Check if claim status = true
            var check = await _itemClaimRepository.FindClaimByItemIdAndUserId(itemId, receiverId);
            if(check == null)
            {
                throw new NoSuchClaimException();
            }
            if (check.IsActive == false)
            {
                throw new CannotAcceptDisabledClaimException();
            }
            //Change Claim to ACCEPTED
            check.ClaimStatus = ClaimStatus.ACCEPTED;
            await _unitOfWork.CommitAsync();
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
                if (claim.UserId != receiverId)
                {
                    //claim.IsActive = false;
                    claim.ClaimStatus = ClaimStatus.DENIED;
                    await _unitOfWork.CommitAsync();
                }
            }
        }

        public async Task<TransferRecordReadDTO> AcceptAClaimAndCreateReceiptAsync(int itemId, string receiverId, IFormFile receiptMedia)
        {
            /*
            + Make a receipt
            + Change item status to RETURNED
            + Set all claims to status = false, except for the claim maker
            */

            //Check userId of claim maker
            var user = await _userRepository.FindUserByID(receiverId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(receiverId);
            }

            //Check if claim status = true
            var check = await _itemClaimRepository.FindClaimByItemIdAndUserId(itemId, receiverId);
            if (check == null)
            {
                throw new NoSuchClaimException();
            }
            if (check.IsActive == false)
            {
                throw new CannotAcceptDisabledClaimException();
            }
            //Change Claim to ACCEPTED
            check.ClaimStatus = ClaimStatus.ACCEPTED;
            await _unitOfWork.CommitAsync();
            //Get item
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }

            //Create a receipt
            var result = await _mediaService.UploadFileAsync(receiptMedia, _awsCredentials);

            //Map ReceiptCreateDTO to ReceiptWriteDTO which has ReceiptImage
            TransferRecordWriteDTO receiptWriteDTO = new TransferRecordWriteDTO()
            {
                ReceiverId = receiverId,
                SenderId = item.FoundUserId,
                ItemId = item.Id,
                ReceiptType = item.IsInStorage == true ? ReceiptType.RETURN_OUT_STORAGE : ReceiptType.RETURN_USER_TO_USER,
                Media = new MediaWriteDTO()
                {
                    Name = receiptMedia.FileName,
                    Description = "Receipt image for item Id = " + item.Id,
                    Url = result.Url,
                }
            };

            var receipt = _mapper.Map<TransferRecord>(receiptWriteDTO);
            receipt.IsActive = true;
            await _receiptRepository.AddAsync(receipt);
            await _unitOfWork.CommitAsync();

            var returnResult = _mapper.Map<TransferRecordReadDTO>(receipt);

            //Change item status to RETURNED
            await UpdateItemStatus(itemId, ItemStatus.RETURNED);
            await _unitOfWork.CommitAsync();

            //Set all claims of this item to status = false
            var claimsOfThisItem = await _itemClaimRepository.GetAllClaimsByItemIdAsync(itemId);
            foreach (var claim in claimsOfThisItem)
            {
                if (claim.UserId != receiverId)
                {
                    //claim.IsActive = false;
                    claim.ClaimStatus = ClaimStatus.DENIED;
                    await _unitOfWork.CommitAsync();
                }
            }

            return returnResult;
        }

        public async Task DenyAClaimAsync(int itemId, string userId)
        {
            //Check userId & check if item exists
            var user = await _userRepository.FindUserByID(userId) ?? throw new EntityWithIDNotFoundException<User>(userId);
            var item = await _itemRepository.FindItemByIdAsync(itemId) ?? throw new EntityWithIDNotFoundException<Item>(itemId);
            //Set this claim to status = false
            var claim = await _itemClaimRepository.FindClaimByItemIdAndUserId(itemId, userId) ?? throw new EntityNotFoundException<ItemClaim>();
            //claim.IsActive = false;
            claim.ClaimStatus = ClaimStatus.DENIED;
            await _unitOfWork.CommitAsync();
        }

        public async Task RevokeDenyAClaimAsync(int itemId, string userId)
        {
            //Check userId & check if item exists
            var user = await _userRepository.FindUserByID(userId) ?? throw new EntityWithIDNotFoundException<User>(userId);
            var item = await _itemRepository.FindItemByIdAsync(itemId) ?? throw new EntityWithIDNotFoundException<Item>(itemId);
            //Check Claim
            var claim = await _itemClaimRepository.FindClaimByItemIdAndUserId(itemId, userId) ?? throw new EntityNotFoundException<ItemClaim>();
            //set claim to pending
            if(claim.ClaimStatus == ClaimStatus.DENIED)
            {
                claim.ClaimStatus = ClaimStatus.PENDING;
            }
            
            await _unitOfWork.CommitAsync();
        }

        public async Task<ItemReadWithReceiptDTO> ReceiveAnItemIntoStorageAsync(string userId, ItemIntoStorageWithReceiptWriteDTO writeDTO)
        {
            //Check if user exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }

            //check if Cabinet exists
            var cabinet = await _cabinetRepository.FindCabinetByIdAsync(writeDTO.CabinetId);
            if (cabinet == null)
            {
                throw new EntityWithIDNotFoundException<Cabinet>(writeDTO.CabinetId);
            }

            //var item = _mapper.Map<Item>(itemWriteDTO);
            var item = new Item()
            {
                Name = writeDTO.Name,
                Description = writeDTO.Description,
                CategoryId = writeDTO.CategoryId,
                LocationId = writeDTO.LocationId,
                CabinetId = writeDTO.CabinetId,
                ItemStatus = ItemStatus.ACTIVE,
                FoundDate = writeDTO.FoundDate,
                FoundUserId = user.Id,
                IsInStorage = true
            };

            /*item.FoundUserId = user.Id;
            item.IsInStorage = true;*/

            await _itemRepository.AddAsync(item);
            await _unitOfWork.CommitAsync();

            //Add Media
            await _itemMediaService.UploadItemMedias(userId, item.Id, writeDTO.ItemMedias);
            await _unitOfWork.CommitAsync();

            //Create Receipt
            var result = await _mediaService.UploadFileAsync(writeDTO.ReceiptMedia, _awsCredentials);

            //Map ReceiptCreateDTO to ReceiptWriteDTO which has ReceiptImage
            TransferRecordWriteDTO receiptWriteDTO = new TransferRecordWriteDTO()
            {
                ReceiverId = userId,
                SenderId = null,
                ItemId = item.Id,
                ReceiptType = ReceiptType.IN_STORAGE,
                Media = new MediaWriteDTO()
                {
                    Name = writeDTO.ReceiptMedia.FileName,
                    Description = "Receipt image for item Id = " + item.Id,
                    Url = result.Url,
                }
            };

            var receipt = _mapper.Map<TransferRecord>(receiptWriteDTO);
            await _receiptRepository.AddAsync(receipt);
            await _unitOfWork.CommitAsync();

            var returnResult = _mapper.Map<ItemReadWithReceiptDTO>(item);
            returnResult.ReceiptId = receipt.Id;
            returnResult.ReceiverId = receipt.ReceiverId;
            returnResult.SenderId = receipt.SenderId;
            returnResult.ReceiptCreatedDate = receipt.CreatedDate;
            returnResult.ReceiptImage = receipt.ReceiptImage;
            returnResult.ReceiptType = receipt.ReceiptType;

            return returnResult;
        }

        public async Task<ItemReadWithReceiptDTO> TransferAnItemIntoStorageAsync(string userId, ItemUpdateTransferToStorageDTO updateDTO)
        {
            //Check if user exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }

            //Check if item exist
            var item = await _itemRepository.FindItemByIdAsync(updateDTO.ItemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(updateDTO.ItemId);
            }
            //check item status
            if(item.IsInStorage == true)
            {
                throw new ItemAlreadyInStorageException();
            }
            if(item.ItemStatus != ItemStatus.PENDING && item.ItemStatus != ItemStatus.ACTIVE)
            {
                throw new ItemNotActiveOrPendingException();
            }

            //check if Cabinet exists
            var cabinet = await _cabinetRepository.FindCabinetByIdAsync(updateDTO.CabinetId);
            if (cabinet == null)
            {
                throw new EntityWithIDNotFoundException<Cabinet>(updateDTO.CabinetId);
            }

            item.ItemStatus = ItemStatus.ACTIVE;
            item.IsInStorage = true;
            item.CabinetId = updateDTO.CabinetId;

            await _unitOfWork.CommitAsync();

            //Create Receipt
            var result = await _mediaService.UploadFileAsync(updateDTO.ReceiptMedia, _awsCredentials);

            //Map ReceiptCreateDTO to ReceiptWriteDTO which has ReceiptImage
            TransferRecordWriteDTO receiptWriteDTO = new TransferRecordWriteDTO()
            {
                ReceiverId = userId,
                SenderId = item.FoundUserId,
                ItemId = item.Id,
                ReceiptType = ReceiptType.IN_STORAGE,
                Media = new MediaWriteDTO()
                {
                    Name = updateDTO.ReceiptMedia.FileName,
                    Description = "Receipt image for item Id = " + item.Id,
                    Url = result.Url,
                }
            };

            var receipt = _mapper.Map<TransferRecord>(receiptWriteDTO);
            await _receiptRepository.AddAsync(receipt);
            await _unitOfWork.CommitAsync();

            var returnResult = _mapper.Map<ItemReadWithReceiptDTO>(item);
            returnResult.ReceiptId = receipt.Id;
            returnResult.ReceiverId = receipt.ReceiverId;
            returnResult.SenderId = receipt.SenderId;
            returnResult.ReceiptCreatedDate = receipt.CreatedDate;
            returnResult.ReceiptImage = receipt.ReceiptImage;
            returnResult.ReceiptType = receipt.ReceiptType;

            return returnResult;
        }
    }
}
