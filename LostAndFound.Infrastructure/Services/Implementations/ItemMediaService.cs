using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.DTOs.UserMedia;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System;
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
    }
}