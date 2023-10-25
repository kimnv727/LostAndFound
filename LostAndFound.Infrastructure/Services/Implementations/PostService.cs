using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPostMediaRepository _postMediaRepository;
        private readonly IPostMediaService _postMediaService;
        private readonly ICommentRepository _commentRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IEmailSendingService _emailSendingService;

        public PostService(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository,
            IPasswordHasherService passwordHasherService, IEmailSendingService emailSendingService, IPostMediaRepository postMediaRepository, IPostMediaService postMediaService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _passwordHasherService = passwordHasherService;
            _emailSendingService = emailSendingService;
            _postMediaRepository = postMediaRepository;
            _postMediaService = postMediaService;
        }

        public async Task UpdatePostStatusAsync(int postId, PostStatus postStatus)
        {
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithIDNotFoundException<Post>(postId);
            }
            post.PostStatus = postStatus;
            await _unitOfWork.CommitAsync();
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await _postRepository.FindPostByIdAsync(postId);

            if (post == null)
            {
                throw new EntityWithIDNotFoundException<Post>(postId);
            }

            _postRepository.Delete(post);
            await _unitOfWork.CommitAsync();
        }

        public async Task<PostDetailWithCommentsReadDTO> GetPostByIdAsync(int postId)
        {
            var post = await _postRepository.FindPostIncludeDetailsAsync(postId);

            if (post == null)
            {
                throw new EntityWithIDNotFoundException<Post>(postId);
            }

            return _mapper.Map<PostDetailWithCommentsReadDTO>(post);
        }
        
        public async Task<IEnumerable<PostReadDTO>> GetPostByUserIdAsync(string userId)
        {
            //Get User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Get Posts
            var posts = await _postRepository.FindAllPostsByUserIdAsync(userId);
            
            return _mapper.Map<PaginatedResponse<PostReadDTO>>(posts);
        }

        public async Task<PaginatedResponse<PostReadDTO>> QueryPostAsync(PostQuery query)
        {
            var posts = await _postRepository.QueryPostAsync(query);
            
            return PaginatedResponse<PostReadDTO>.FromEnumerableWithMapping(posts, query, _mapper);
        }

        public async Task<PaginatedResponse<PostDetailReadDTO>> QueryPostWithStatusAsync(PostQueryWithStatus query)
        {
            var posts = await _postRepository.QueryPostWithStatusAsync(query);
            
            return PaginatedResponse<PostDetailReadDTO>.FromEnumerableWithMapping(posts, query, _mapper);
        }

        public async Task<PostDetailReadDTO> CreatePostAsync(string userId, PostWriteDTO postWriteDTO)
        {
            //Get User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Map Post
            var post = _mapper.Map<Post>(postWriteDTO);
            post.PostUserId = userId;
            post.PostStatus = PostStatus.PENDING;
            //Add Post
            await _postRepository.AddAsync(post);
            await _unitOfWork.CommitAsync();

            //AddMedia
            await _postMediaService.UploadPostMedias(userId, post.Id, postWriteDTO.Medias);
            await _unitOfWork.CommitAsync();

            var postReadDTO = _mapper.Map<PostDetailReadDTO>(post);
            return postReadDTO;
        }

        public async Task<PostDetailReadDTO> UpdatePostDetailsAsync(int postId, PostUpdateDTO postUpdateDTO)
        {
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithIDNotFoundException<Post>(postId);
            }
            _mapper.Map(postUpdateDTO, post);
            if(post.PostStatus == PostStatus.REJECTED)
            {
                post.PostStatus = PostStatus.PENDING;
            }
            await _unitOfWork.CommitAsync();
            return _mapper.Map<PostDetailReadDTO>(post);
        }

        public async Task<bool> CheckPostAuthorAsync(int postId, string userId)
        {
            var post = await _postRepository.FindPostByIdAndUserId(postId, userId);

            if (post == null)
                throw new EntityWithIDNotFoundException<Post>(postId);

            return post != null ? true : false;
        }
    }
}