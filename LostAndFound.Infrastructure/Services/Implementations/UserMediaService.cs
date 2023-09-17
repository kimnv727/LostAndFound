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
    public class UserMediaService : IUserMediaService
    {
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AwsCredentials _awsCredentials;
        private readonly IUserMediaRepository _userMediaRepository;
        private readonly IUserRepository _userRepository;

        public UserMediaService(IMapper mapper, IMediaService mediaService, IUnitOfWork unitOfWork, AwsCredentials awsCredentials, IUserMediaRepository userMediaRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _mediaService = mediaService;
            _unitOfWork = unitOfWork;
            _awsCredentials = awsCredentials;
            _userMediaRepository = userMediaRepository;
            _userRepository = userRepository;
        }

        public async Task<UserMediaReadDTO> UploadUserAvatar(IFormFile file, string userId)
        {
            //check for user
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithAttributeNotFoundException<User>(nameof(User.Id), userId);
            }

            //Upload the file
            //Deactive old avatar
            var currentAvatar = await _userMediaRepository.FindUserMediaIncludeMediaAsync(userId);
            if(currentAvatar != null)
            {
                currentAvatar.Media.IsActive = false;
                currentAvatar.Media.DeletedDate = DateTime.Now.ToVNTime();
                await _unitOfWork.CommitAsync();
            }


            //Upload and update new avatar
            var result = await _mediaService.UploadFileAsync(file, _awsCredentials);
            UserMedia userMedia = new UserMedia()
            {
                UserId = userId,
                Media = new Media()
                {
                    Name = file.FileName,
                    Description = "Avatar of " + user.Email,
                    URL = result.Url,
                }
            };

            await _userMediaRepository.AddAsync(userMedia);
            user.Avatar = result.Url;
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UserMediaReadDTO>(userMedia);
        }
    }
}
