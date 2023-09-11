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
    public class PostMediaService : IPostMediaService
    {
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AwsCredentials _awsCredentials;
        private readonly IPostMediaRepository _postMediaRepository;
        private readonly IPostRepository _postRepository;

        public PostMediaService(IMapper mapper, IMediaService mediaService, IUnitOfWork unitOfWork, AwsCredentials awsCredentials, IPostRepository postRepository, IPostMediaRepository postMediaRepository)
        {
            _mapper = mapper;
            _mediaService = mediaService;
            _unitOfWork = unitOfWork;
            _awsCredentials = awsCredentials;
            _postRepository = postRepository;
            _postMediaRepository = postMediaRepository;
        }
    }
}
