using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Authenticate;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.DTOs.ItemMedia;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.DTOs.UserMedia;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class ItemMediaService : IItemMediaService
    {
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AwsCredentials _awsCredentials;
        private readonly IItemMediaRepository _itemMediaRepository;
        private readonly IItemRepository _itemRepository;

        public ItemMediaService(IMapper mapper, IMediaService mediaService, IUnitOfWork unitOfWork, AwsCredentials awsCredentials, IItemRepository itemRepository, IItemMediaRepository itemMediaRepository)
        {
            _mapper = mapper;
            _mediaService = mediaService;
            _unitOfWork = unitOfWork;
            _awsCredentials = awsCredentials;
            _itemRepository = itemRepository;
            _itemMediaRepository = itemMediaRepository;
        }

        public async Task DeleteItemMedia(string userId, int itemId, Guid mediaId)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithAttributeNotFoundException<Item>(nameof(Item.Id), itemId);
            }

            if (item.FoundUserId != userId)
            {
                throw new UserNotPermittedToResourceException<Item>();
            }

            var media = await _mediaService.FindMediaById(mediaId);
            if (media == null)
            {
                throw new EntityWithAttributeNotFoundException<Media>(nameof(Media.Id), mediaId);
            }

            await _mediaService.DeleteMediaAsync(mediaId);
        }

        public async Task<IEnumerable<ItemMediaReadDTO>> GetItemMedias(int itemId)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithAttributeNotFoundException<Item>(nameof(Item.Id), itemId);
            }

            IEnumerable<ItemMedia> im = await _itemMediaRepository.FindItemMediaIncludeMediaAsync(itemId);
            return _mapper.Map<List<ItemMediaReadDTO>>(im.ToList());
        }

        public async Task<IEnumerable<ItemMediaReadDTO>> UploadItemMedias(string userId, int itemId, IFormFile[] files)
        {
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithAttributeNotFoundException<Post>(nameof(Item.Id), itemId);
            }

            if (item.FoundUserId != userId)
            {
                throw new UserNotPermittedToResourceException<Item>();
            }

            List<S3ReponseDTO> uploadResult = new List<S3ReponseDTO>();
            string[] filename = files.Select(f => f.FileName).ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                var upload = await _mediaService.UploadFileAsync(files[i], _awsCredentials);
                uploadResult.Add(upload);
            }

            //TODO: Item posted on website should be moderated so add PENDING STATUS please
            //item.ItemStatus = Core.Enums.ItemStatus.;
            //await _unitOfWork.CommitAsync();

            List<ItemMedia> itemMedias = new List<ItemMedia>();
            int j = 0;
            foreach (var ul in uploadResult)
            {

                ItemMedia im = new ItemMedia()
                {
                    ItemId = itemId,
                    Media = new Media()
                    {
                        Name = filename[j],
                        Description = "Image from post with id " + itemId,
                        URL = ul.Url
                    }
                };
                j++;
                await _itemMediaRepository.AddAsync(im);
                itemMedias.Add(im);
            }
            await _unitOfWork.CommitAsync();
            return _mapper.Map<IEnumerable<ItemMediaReadDTO>>(itemMedias);
        }
    }
}