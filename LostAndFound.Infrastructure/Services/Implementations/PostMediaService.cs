using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Exceptions.Authenticate;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.DTOs.PostMedia;
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

        public async Task DeletePostMedia(string userId, int postId, Guid mediaId)
        {
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithAttributeNotFoundException<Post>(nameof(Post.Id), postId);
            }

            if (post.PostUserId != userId)
            {
                throw new UserNotPermittedToResourceException<Post>();
            }

            var media = await _mediaService.FindMediaById(mediaId);
            if (media == null)
            {
                throw new EntityWithAttributeNotFoundException<Media>(nameof(Media.Id), mediaId);
            }

            await _mediaService.DeleteMediaAsync(mediaId);
        }

        public async Task<IEnumerable<PostMediaReadDTO>> GetPostMedias(int postId)
        {
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithAttributeNotFoundException<Post>(nameof(Post.Id), postId);
            }

            IEnumerable<PostMedia> pm = await _postMediaRepository.FindPostMediaIncludeMediaAsync(postId);
            return _mapper.Map<List<PostMediaReadDTO>>(pm.ToList());
        }

        public async Task<IEnumerable<PostMediaReadDTO>> UploadPostMedias(string userId, int postId, IFormFile[] files)
        {
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithAttributeNotFoundException<Post>(nameof(Post.Id), postId);
            }

            if(post.PostUserId != userId)
            {
                throw new UserNotPermittedToResourceException<Post>();
            }

            List<S3ReponseDTO> uploadResult = new List<S3ReponseDTO>();
            string[] filename = files.Select(f => f.FileName).ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                var upload = await _mediaService.UploadFileAsync(files[i], _awsCredentials);
                uploadResult.Add(upload);
            }

            post.PostStatus = Core.Enums.PostStatus.PENDING;
            await _unitOfWork.CommitAsync();

            List<PostMedia> postMedias = new List<PostMedia>();
            int j = 0;
            foreach(var ul in uploadResult)
            {

                PostMedia pm = new PostMedia()
                {
                    PostId = postId,
                    Media = new Media()
                    {
                        Name = filename[j],
                        Description = "Image from post with id " + postId,
                        URL = ul.Url
                    }
                };
                j++;
                await _postMediaRepository.AddAsync(pm);
                postMedias.Add(pm);
            }
            await _unitOfWork.CommitAsync();
            return _mapper.Map<IEnumerable<PostMediaReadDTO>>(postMedias);
        }
    }
}
