using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Authenticate;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Exceptions.Giveaway;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Location;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.DTOs.Receipt;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMediaService _mediaService;
        private readonly IMediaRepository _mediaRepository;
        private readonly IGiveawayRepository _giveawayRepository;
        private readonly IReportRepository _reportRepository;
        private readonly AwsCredentials _awsCredentials;

        public ReceiptService(IReceiptRepository receiptRepository, IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, 
            IItemRepository itemRepository, IMediaService mediaService, AwsCredentials awsCredentials, IMediaRepository mediaRepository,
            IGiveawayRepository giveawayRepository, IReportRepository reportRepository)
        {
            _receiptRepository = receiptRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _mediaService = mediaService;
            _awsCredentials = awsCredentials;
            _mediaRepository = mediaRepository;
            _giveawayRepository = giveawayRepository;
            _reportRepository = reportRepository;
        }

        public async Task<PaginatedResponse<TransferRecordReadDTO>> QueryReceiptAsync(TransferRecordQuery query)
        {
            var receipts = await _receiptRepository.QueryReceiptAsync(query);
            //
            return PaginatedResponse<TransferRecordReadDTO>.FromEnumerableWithMapping(receipts, query, _mapper);
        }

        public async Task<IEnumerable<TransferRecordReadDTO>> ListAllAsync()
        {
            var receipts = await _receiptRepository.GetAllWithMediaAsync();
            //
            return _mapper.Map<List<TransferRecordReadDTO>>(receipts);
        }

        public async Task<TransferRecordReadWithUserDTO> FindReceiptByIdAsync(int receiptId)
        {
            var receipt = await _receiptRepository.GetReceiptByIdAsync(receiptId);
            if (receipt == null)
            {
                throw new EntityWithIDNotFoundException<TransferRecord>(receiptId);
            }

            /*return _mapper.Map<TransferRecordReadDTO>(receipt);*/
            //Get Receipts
            var result = _mapper.Map<TransferRecordReadWithUserDTO>(receipt);
            result.ReceiverUser = _mapper.Map<UserReadDTO>(await _userRepository.FindUserByID(result.ReceiverId));
            result.SenderUser = _mapper.Map<UserReadDTO>(await _userRepository.FindUserByID(result.SenderId));

            return result;
        }

        public async Task DeleteReceiptAsync(int receiptId)
        {
            var receipt = await _receiptRepository.GetReceiptByIdAsync(receiptId);
            if(receipt == null)
            {
                throw new EntityWithIDNotFoundException<TransferRecord>(receiptId);
            }

            var image = await _mediaRepository.FindMediaByIdAsync(receipt.ReceiptImage);
            if (image == null)
            {
                throw new EntityWithIDNotFoundException<Media>(receipt.ReceiptImage);
            }
            //Delete media then delete receipt
            _mediaRepository.Delete(image);
            _receiptRepository.Delete(receipt);

            await _unitOfWork.CommitAsync();
        }

        public async Task<TransferRecordReadDTO> CreateReceiptAsync(TransferRecordCreateDTO receiptCreateDTO,  IFormFile image)
        {
            //Check null for receiver,sender & item 
            var receiver = await _userRepository.FindUserByID(receiptCreateDTO.ReceiverId);
            if (receiver == null)
            {
                throw new EntityWithIDNotFoundException<User>(receiptCreateDTO.ReceiverId);
            }

            var sender = receiptCreateDTO.SenderId;

            if (!string.IsNullOrEmpty(sender))
            {
                var check = await _userRepository.FindUserByID(receiptCreateDTO.SenderId);
                if (check == null)
                {
                        throw new EntityWithIDNotFoundException<User>(check);                   
                }
            }

            var item = await _itemRepository.FindItemByIdAsync(receiptCreateDTO.ItemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(receiptCreateDTO.ItemId);
            }

            var result = await _mediaService.UploadFileAsync(image, _awsCredentials);

            //Map ReceiptCreateDTO to ReceiptWriteDTO which has ReceiptImage
            TransferRecordWriteDTO receiptWriteDTO = new TransferRecordWriteDTO()
            {
                ReceiverId = receiptCreateDTO.ReceiverId,
                SenderId = receiptCreateDTO.SenderId,
                ItemId = receiptCreateDTO.ItemId,
                ReceiptType = receiptCreateDTO.ReceiptType,
                Media = new MediaWriteDTO()
                {
                    Name = image.FileName,
                    Description = "Receipt image for item Id = " + receiptCreateDTO.ItemId,
                    Url = result.Url,
                }
            };

            var receipt = _mapper.Map<TransferRecord>(receiptWriteDTO);
            receipt.IsActive = true;
            await _receiptRepository.AddAsync(receipt);

            if(item.ItemStatus == ItemStatus.ONHOLD)
            {
                item.ItemStatus = ItemStatus.RETURNED;
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<TransferRecordReadDTO>(receipt);
        }

        public async Task<IEnumerable<TransferRecordReadWithUserDTO>> GetAllReceiptsByItemIdAsync(int itemId)
        {
            //Get Item
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }
            /*//Get Receipts
            var receipts = await _receiptRepository.GetAllWithItemIdAsync(itemId);

            return _mapper.Map<List<TransferRecordReadDTO>>(receipts);*/

            //Get Receipts
            var receipts = await _receiptRepository.GetAllWithItemIdAsync(itemId);
            var result = _mapper.Map<List<TransferRecordReadWithUserDTO>>(receipts);
            foreach (var r in result)
            {
                r.ReceiverUser = _mapper.Map<UserReadDTO>(await _userRepository.FindUserByID(r.ReceiverId));
                r.SenderUser = _mapper.Map<UserReadDTO>(await _userRepository.FindUserByID(r.SenderId));
            }

            return result;
        }

        public async Task<IEnumerable<TransferRecordReadWithUserDTO>> GetReceiptsByUserIdAsync(string userId)
        {
            //Check null for user
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Get Receipts
            var receipts = await _receiptRepository.GetReceiptsByUserIdAsync(userId);
            var result = _mapper.Map<List<TransferRecordReadWithUserDTO>>(receipts);
            foreach (var r in result)
            {
                r.ReceiverUser = _mapper.Map<UserReadDTO>(await _userRepository.FindUserByID(r.ReceiverId));
                r.SenderUser = _mapper.Map<UserReadDTO>(await _userRepository.FindUserByID(r.SenderId));
            }

            return result;
        }

        public async Task<TransferRecordReadDTO> RevokeReceipt(int receiptId)
        {
            var receipt = await _receiptRepository.GetReceiptByIdAsync(receiptId);
            if (receipt == null)
            {
                throw new EntityWithIDNotFoundException<TransferRecord>(receiptId);
            }

            //check if receipt is return type
            if(receipt.ReceiptType != ReceiptType.RETURN_OUT_STORAGE && receipt.ReceiptType != ReceiptType.RETURN_USER_TO_USER)
            {
                throw new NotPermittedException("Receipt of this type cannot be revoked!");
            }
            //change receipt isActive to false
            receipt.IsActive = false;
            //get item and change their status back to Active
            var item = await _itemRepository.FindItemByIdAsync(receipt.ItemId);
            item.ItemStatus = ItemStatus.ACTIVE;

            await _unitOfWork.CommitAsync();
            return _mapper.Map<TransferRecordReadDTO>(receipt);
        }

        public async Task<TransferRecordReadDTO> CreateReceiptForGiveawayAsync(string currentUserId, TransferRecordGiveawayCreateDTO receiptCreateDTO, IFormFile image)
        {
            //Check null for receiver,sender & item 
            var receiver = await _userRepository.FindUserByID(receiptCreateDTO.ReceiverId);
            if (receiver == null)
            {
                throw new EntityWithIDNotFoundException<User>(receiptCreateDTO.ReceiverId);
            }

            //check Giveaway 
            var giveaway = await _giveawayRepository.FindGiveawayByIdAsync(receiptCreateDTO.GiveawayId);
            if (giveaway == null)
            {
                throw new EntityWithIDNotFoundException<Giveaway>(receiptCreateDTO.GiveawayId);
            }

            //check availability
            if(giveaway.GiveawayStatus != GiveawayStatus.REWARD_DISTRIBUTION_IN_PROGRESS)
            {
                throw new InvalidGiveawayException();
            }

            //check Item
            var item = await _itemRepository.FindItemByIdAsync(giveaway.ItemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(giveaway.ItemId);
            }

            var result = await _mediaService.UploadFileAsync(image, _awsCredentials);

            //Map ReceiptCreateDTO to ReceiptWriteDTO which has ReceiptImage
            TransferRecordWriteDTO receiptWriteDTO = new TransferRecordWriteDTO()
            {
                ReceiverId = receiptCreateDTO.ReceiverId,
                SenderId = currentUserId,
                ItemId = item.Id,
                ReceiptType = ReceiptType.GIVEAWAY_OUT_STORAGE,
                Media = new MediaWriteDTO()
                {
                    Name = image.FileName,
                    Description = "Receipt image for giving away item Id = " + item.Id,
                    Url = result.Url,
                }
            };

            var receipt = _mapper.Map<TransferRecord>(receiptWriteDTO);
            receipt.IsActive = true;
            //closed giveaway
            giveaway.GiveawayStatus = GiveawayStatus.CLOSED;
            giveaway.Item.ItemStatus = ItemStatus.GAVEAWAY;
            //item
            item.IsInStorage = false;

            await _receiptRepository.AddAsync(receipt);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<TransferRecordReadDTO>(receipt);
        }

        public async Task<TransferRecordReadDTO> CreateReceiptForOnHoldItemAsync(string currentUserId, TransferRecordOnholdItemCreateDTO receiptCreateDTO, IFormFile image)
        {
            //Check null for receiver,sender & item 
            var sender = await _userRepository.FindUserByID(receiptCreateDTO.SenderId);
            if (sender == null)
            {
                throw new EntityWithIDNotFoundException<User>(receiptCreateDTO.SenderId);
            }

            //check Report 
            var report = await _reportRepository.GetReportByIdAsync(receiptCreateDTO.ReportId);
            if (report == null)
            {
                throw new EntityWithIDNotFoundException<Report>(receiptCreateDTO.ReportId);
            }

            //check Item
            var item = await _itemRepository.FindItemByIdAsync(report.ItemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(report.ItemId);
            }

            var result = await _mediaService.UploadFileAsync(image, _awsCredentials);

            //Map ReceiptCreateDTO to ReceiptWriteDTO which has ReceiptImage
            TransferRecordWriteDTO receiptWriteDTO = new TransferRecordWriteDTO()
            {
                ReceiverId = currentUserId,
                SenderId = receiptCreateDTO.SenderId,
                ItemId = item.Id,
                ReceiptType = ReceiptType.IN_STORAGE,
                Media = new MediaWriteDTO()
                {
                    Name = image.FileName,
                    Description = "Receipt image for giving away item Id = " + item.Id,
                    Url = result.Url,
                }
            };
            //change item status to onhold + in storage + cabinet
            item.ItemStatus = ItemStatus.ONHOLD;
            item.IsInStorage = true;
            item.CabinetId = receiptCreateDTO.CabinetId;

            var receipt = _mapper.Map<TransferRecord>(receiptWriteDTO);
            receipt.IsActive = true;

            //send email to the user B to go up

            await _receiptRepository.AddAsync(receipt);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<TransferRecordReadDTO>(receipt);
        }
    }
}
