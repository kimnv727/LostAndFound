using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Location;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.DTOs.Receipt;
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
        private readonly AwsCredentials _awsCredentials;

        public ReceiptService(IReceiptRepository receiptRepository, IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IItemRepository itemRepository, IMediaService mediaService, AwsCredentials awsCredentials, IMediaRepository mediaRepository)
        {
            _receiptRepository = receiptRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _mediaService = mediaService;
            _awsCredentials = awsCredentials;
            _mediaRepository = mediaRepository;
        }

        public async Task<PaginatedResponse<ReceiptReadDTO>> QueryReceiptAsync(ReceiptQuery query)
        {
            var receipts = await _receiptRepository.QueryReceiptAsync(query);
            //
            return PaginatedResponse<ReceiptReadDTO>.FromEnumerableWithMapping(receipts, query, _mapper);
        }

        public async Task<IEnumerable<ReceiptReadDTO>> ListAllAsync()
        {
            var receipts = await _receiptRepository.GetAllWithMediaAsync();
            //
            return _mapper.Map<List<ReceiptReadDTO>>(receipts);
        }

        public async Task<ReceiptReadDTO> FindReceiptByIdAsync(int receiptId)
        {
            var receipt = await _receiptRepository.GetReceiptByIdAsync(receiptId);
            if (receipt == null)
            {
                throw new EntityWithIDNotFoundException<Receipt>(receiptId);
            }

            return _mapper.Map<ReceiptReadDTO>(receipt);
        }

        public async Task DeleteReceiptAsync(int receiptId)
        {
            var receipt = await _receiptRepository.GetReceiptByIdAsync(receiptId);
            if(receipt == null)
            {
                throw new EntityWithIDNotFoundException<Receipt>(receiptId);
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

        public async Task<ReceiptReadDTO> CreateReceiptAsync(ReceiptCreateDTO receiptCreateDTO,  IFormFile image)
        {
            //Check null for receiver,sender & item 
            var receiver = await _userRepository.FindUserByID(receiptCreateDTO.ReceiverId);
            if (receiver == null)
            {
                throw new EntityWithIDNotFoundException<User>(receiptCreateDTO.ReceiverId);
            }
            var sender = await _userRepository.FindUserByID(receiptCreateDTO.SenderId);
            if (sender == null)
            {
                throw new EntityWithIDNotFoundException<User>(receiptCreateDTO.SenderId);
            }
            var item = await _itemRepository.FindItemByIdAsync(receiptCreateDTO.ItemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(receiptCreateDTO.ItemId);
            }

            var result = await _mediaService.UploadFileAsync(image, _awsCredentials);

            //Map ReceiptCreateDTO to ReceiptWriteDTO which has ReceiptImage
            ReceiptWriteDTO receiptWriteDTO = new ReceiptWriteDTO()
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

            var receipt = _mapper.Map<Receipt>(receiptWriteDTO);
            await _receiptRepository.AddAsync(receipt);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<ReceiptReadDTO>(receipt);
        }

    }
}
