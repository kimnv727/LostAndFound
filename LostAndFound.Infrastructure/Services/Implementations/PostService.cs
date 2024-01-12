using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using F23.StringSimilarity;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.DTOs.Category;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.Location;
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
        private readonly IItemRepository _itemRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILocationRepository _locationRepository;

        public PostService(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository,
            IPasswordHasherService passwordHasherService, IEmailSendingService emailSendingService, IPostMediaRepository postMediaRepository, 
            IPostMediaService postMediaService, IItemRepository itemRepository, ICategoryRepository categoryRepository, ILocationRepository locationRepository)
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
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
            _locationRepository = locationRepository;
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

            if (postStatus == PostStatus.DELETED)
            {
                _emailSendingService.SendMailWhenPostBan(post.User.Email, post.Title);
            }
            if (postStatus == PostStatus.ACTIVE)
            {
                _emailSendingService.SendMailPostApprove(post.User.Email, post.Title);
            }
            if (postStatus == PostStatus.REJECTED)
            {
                _emailSendingService.SendMailPostReject(post.User.Email, post.Title);
            }
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

            _emailSendingService.SendMailWhenPostBan(post.User.Email, post.Title);
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

        public async Task<PaginatedResponse<PostDetailReadDTO>> QueryPostWithStatusExcludePendingAndRejectedAsync(PostQueryWithStatusExcludePendingAndRejected query)
        {
            var posts = await _postRepository.QueryPostWithStatusExcludePendingAndRejectedAsync(query);

            return PaginatedResponse<PostDetailReadDTO>.FromEnumerableWithMapping(posts, query, _mapper);
        }

        public async Task<PaginatedResponse<PostDetailWithFlagReadDTO>> QueryPostWithFlagAsync(PostQueryWithFlag query)
        {
            var posts = await _postRepository.QueryPostWithFlagAsync(query);
            var result = new List<PostDetailWithFlagReadDTO>();

            foreach (var p in posts)
            {
                var r = _mapper.Map<PostDetailWithFlagReadDTO>(p);
                r.WrongInformationCount = p.PostFlags.Where(p => p.PostFlagReason == PostFlagReason.FALSE_INFORMATION && p.IsActive == true).Count();
                r.SpamCount = p.PostFlags.Where(p => p.PostFlagReason == PostFlagReason.SPAM && p.IsActive == true).Count();
                r.ViolatedUserCount = p.PostFlags.Where(p => p.PostFlagReason == PostFlagReason.VIOLATED_USER_POLICIES && p.IsActive == true).Count();
                //r.OthersCount = p.PostFlags.Where(p => p.PostFlagReason == PostFlagReason.Others && p.IsActive == true).Count();
                r.TotalCount = p.PostFlags.Where(p => p.IsActive == true).Count();
                result.Add(r);
            }

            return PaginatedResponse<PostDetailWithFlagReadDTO>.FromEnumerableWithMapping(result, query, _mapper);
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

            //Add Location & Category
            //Get Location
            if (!string.IsNullOrWhiteSpace(postWriteDTO.PostLocation))
            {
                var locationIdList = Array.ConvertAll(postWriteDTO.PostLocation.Substring(1, postWriteDTO.PostLocation.Length - 2).Split('|'), int.Parse);
                var locationObjectList = await _locationRepository.GetAllWithCampusByIdArrayAsync(locationIdList);
                post.Locations = (ICollection<Location>)locationObjectList; 
            }
            //Get Category
            if (!string.IsNullOrWhiteSpace(postWriteDTO.PostCategory))
            {
                var categoryIdList = Array.ConvertAll(postWriteDTO.PostCategory.Substring(1, postWriteDTO.PostCategory.Length - 2).Split('|'), int.Parse);
                var categoryObjectList = await _categoryRepository.GetAllWithGroupsByIdArrayAsync(categoryIdList);
                post.Categories = (ICollection<Category>)categoryObjectList;
            }

            //Add Post
            await _postRepository.AddAsync(post);
            await _unitOfWork.CommitAsync();

            //AddMedia
            await _postMediaService.UploadPostMedias(userId, post.Id, postWriteDTO.Medias);
            await _unitOfWork.CommitAsync();

            var result = _mapper.Map<PostDetailReadDTO>(post);

            return result;
        }

        public async Task<PostDetailReadDTO> UpdatePostDetailsAsync(int postId, PostUpdateDTO postUpdateDTO)
        {
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithIDNotFoundException<Post>(postId);
            }
            _mapper.Map(postUpdateDTO, post);

            //Clear old
            post.Locations.Clear();
            post.Categories.Clear();
            //Add new
            //Get Location
            if (!string.IsNullOrWhiteSpace(postUpdateDTO.PostLocation))
            {
                var locationIdList = Array.ConvertAll(postUpdateDTO.PostLocation.Substring(1, postUpdateDTO.PostLocation.Length - 2).Split('|'), int.Parse);
                var locationObjectList = await _locationRepository.GetAllWithCampusByIdArrayAsync(locationIdList);
                post.Locations = (ICollection<Location>)locationObjectList;
            }
            //Get Category
            if (!string.IsNullOrWhiteSpace(postUpdateDTO.PostCategory))
            {
                var categoryIdList = Array.ConvertAll(postUpdateDTO.PostCategory.Substring(1, postUpdateDTO.PostCategory.Length - 2).Split('|'), int.Parse);
                var categoryObjectList = await _categoryRepository.GetAllWithGroupsByIdArrayAsync(categoryIdList);
                post.Categories = (ICollection<Category>)categoryObjectList;
            }

            if (post.PostStatus == PostStatus.REJECTED)
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

        public async Task<PostReadDTO> RecommendMostRelatedPostAsync(int itemId)
        {
            //Get Item
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }

            //Get Related Post
            var posts = await _postRepository.GetPostsByLocationAndCategoryAsync(item.LocationId, item.CategoryId);

            //String similarity
            if(posts.Count() > 0)
            {
                var jw = new JaroWinkler();
                foreach (var p in posts)
                {
                    if(jw.Similarity(p.Title, item.Name) > 0.8 && jw.Similarity(p.PostContent, item.Description) > 0.8)
                    {
                        return _mapper.Map<PostReadDTO>(p);
                    }
                }
            }

            return null;
        }
    }
}