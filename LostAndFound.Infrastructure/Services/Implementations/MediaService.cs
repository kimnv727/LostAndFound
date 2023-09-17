using Amazon.S3;
using Amazon.Runtime;
using Amazon.S3.Transfer;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.UnitOfWork;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class MediaService : IMediaService
    {
        
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediaRepository _mediaRepository;

        public MediaService(IMediaRepository mediaRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mediaRepository = mediaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DTOs.Common.PaginatedResponse<MediaReadDTO>> QueryMediaAsync(MediaQuery query)
        {
            //query
            var medias = await _mediaRepository.QueryMediaAsync(query);

            return DTOs.Common.PaginatedResponse<MediaReadDTO>.FromEnumerableWithMapping(medias, query, _mapper);
        }

        public async Task<DTOs.Common.PaginatedResponse<MediaDetailReadDTO>> QueryMediaIgnoreStatusAsync(MediaQueryWithStatus query)
        {
            //query
            var medias = await _mediaRepository.QueryMediaIgnoreStatusAsync(query);

            return DTOs.Common.PaginatedResponse<MediaDetailReadDTO>.FromEnumerableWithMapping(medias, query, _mapper);
        }

        public async Task<MediaReadDTO> FindMediaById(Guid mediaId)
        {
            var media = await _mediaRepository.FindMediaByIdAsync(mediaId);
            if (media == null)
            {
                throw new EntityWithIDNotFoundException<Media>(mediaId);
            }
            return _mapper.Map<MediaReadDTO>(media);
        }

        //TODO: Update bucket profile and token
        public async Task<S3ReponseDTO> UploadFileAsync(IFormFile file, AwsCredentials awsCredentialsValues)
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileExt = Path.GetExtension(file.FileName);
            CheckMediaExtensions(fileExt);
            var docName = $"upload/{Guid.NewGuid()}{fileExt}";

            //var awsCredentialsValues = _config.ReadS3Credentials();

            Console.WriteLine($"Key: {awsCredentialsValues.AccessKey}, Secret: {awsCredentialsValues.SecretKey}");

            var credentials = new BasicAWSCredentials(awsCredentialsValues.AccessKey, awsCredentialsValues.SecretKey);

            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.APNortheast1
            };

            var response = new S3ReponseDTO();

            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = memoryStream,
                Key = docName,
                BucketName = awsCredentialsValues.BucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            // initialise client
            using var client = new AmazonS3Client(credentials, config);

            // initialise the transfer/upload tools
            var transferUtility = new TransferUtility(client);

            // initiate the file upload
            await transferUtility.UploadAsync(uploadRequest);

            response.Url = "https://" + awsCredentialsValues.BucketName + ".s3.ap-northeast-1.amazonaws.com/" + docName;
            return response;
        }

        private void CheckMediaExtensions(string ext)
        {
            if (ext.Equals(".png") ||
                ext.Equals(".jpeg") ||
                ext.Equals(".jpg") ||
                ext.Equals(".jfif") ||
                ext.Equals(".mp4") ||
                ext.Equals(".mkv"))
            {
            }
            else
            {
                throw new InvalidFileFormatException("image or video");
            }
        }

        public async Task<MediaReadDTO> UpdateMediaDetail(Guid mediaId, MediaUpdateWriteDTO mediaUpdateWriteDTO)
        {
            var media = await _mediaRepository.FindMediaByIdAsync(mediaId);
            if (media == null || media.IsActive == false)
            {
                throw new EntityWithIDNotFoundException<Media>(mediaId);
            }
            _mapper.Map(mediaUpdateWriteDTO, media);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<MediaReadDTO>(media);
        }

        public async Task UpdateMediaStatus(Guid mediaId)
        {
            var media = await _mediaRepository.FindMediaByIdAsync(mediaId);
            if (media == null)
            {
                throw new EntityWithIDNotFoundException<Media>(mediaId);
            }
            if (media.IsActive == true)
            {
                _mediaRepository.Delete(media);
            }
            else if (media.IsActive == false)
            {
                media.IsActive = true;
                media.DeletedDate = null;
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteMediaAsync(Guid mediaId)
        {
            var media = await _mediaRepository.FindAsync(mediaId);

            if (media == null)
            {
                throw new EntityWithIDNotFoundException<Media>(mediaId);
            }

            _mediaRepository.Delete(media);
            await _unitOfWork.CommitAsync();
        }
        
    }
}
