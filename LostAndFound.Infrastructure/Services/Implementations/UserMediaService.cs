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
using System.Collections.Generic;
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
            var currentAvatar = await _userMediaRepository.FindUserMediaWithOnlyAvatarAsync(userId);
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
                MediaType = Core.Enums.UserMediaType.AVATAR,
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

        public async Task<ICollection<UserMediaReadDTO>> UploadUserCredentialForVerification(string userId, IFormFile ccid, IFormFile studentCard)
        {
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithAttributeNotFoundException<User>(nameof(User.Id), userId);
            }

            if(user.VerifyStatus == Core.Enums.UserVerifyStatus.VERIFIED)
            {
                throw new Exception(); //TODO: <<Exception>>: say the user already verified, so they are not allowed to use this
            }

            //Deactivate old credential images 
            List<UserMedia> currentCredentials = (List<UserMedia>)await _userMediaRepository.FindUserMediaWithMediasExceptAvatarAsync(userId);
            foreach(var cred in currentCredentials)
            {
                cred.Media.IsActive = false;
                cred.Media.DeletedDate = DateTime.Now.ToVNTime();
            }
            await _unitOfWork.CommitAsync();

            //Upload images
            var ccidResult = await _mediaService.UploadFileAsync(ccid, _awsCredentials);
            var studentCardResult = await _mediaService.UploadFileAsync(studentCard, _awsCredentials);
            UserMedia ccidMedia = new UserMedia()
            {
                UserId = userId,
                MediaType = Core.Enums.UserMediaType.IDENTIFICATION_CARD,
                Media = new Media()
                {
                    Name = ccid.FileName,
                    Description = "CCID of " + user.Email,
                    URL = ccidResult.Url,
                }
            };

            UserMedia studentCardMedia = new UserMedia()
            {
                UserId = userId,
                MediaType = Core.Enums.UserMediaType.STUDENT_CARD,
                Media = new Media()
                {
                    Name = studentCard.FileName,
                    Description = "Student Card of " + user.Email,
                    URL = studentCardResult.Url,
                }
            };

            //Save in db
            await _userMediaRepository.AddAsync(ccidMedia);
            await _userMediaRepository.AddAsync(studentCardMedia);
            await _unitOfWork.CommitAsync();

            //Change user status to waiting verified
            user.VerifyStatus = Core.Enums.UserVerifyStatus.WAITING_VERIFIED;
            await _unitOfWork.CommitAsync();

            //return the the 2 credentials images
            List<UserMediaReadDTO> credentials = new List<UserMediaReadDTO>();
            credentials.Add(_mapper.Map<UserMediaReadDTO>(ccidMedia));
            credentials.Add(_mapper.Map<UserMediaReadDTO>(studentCardMedia));

            return credentials;
        }
    }
}
