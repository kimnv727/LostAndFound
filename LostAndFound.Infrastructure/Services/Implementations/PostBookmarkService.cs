using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.ItemMedia;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.DTOs.PostBookmark;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Implementations;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class PostBookmarkService : IPostBookmarkService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPostBookmarkRepository _postBookmarkRepository;

        public PostBookmarkService(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository,
            IPostRepository postRepository, IPostBookmarkRepository postBookmarkRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _postBookmarkRepository = postBookmarkRepository;
        }
        
        public async Task<int> CountPostBookmarkAsync(int postId)
        {
            //check Post
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithIDNotFoundException<Post>(postId);
            }
            //count
            var result = await _postBookmarkRepository.CountPostBookmarkAsync(postId);

            return result;
        }
        
        public async Task<PostBookmarkReadDTO> GetPostBookmark(string userId, int postId)
        {
            //check User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //check Post
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithIDNotFoundException<Post>(postId);
            }
            
            var postBookmark = await _postBookmarkRepository.FindPostBookmarkAsync(postId, userId);

            if (postBookmark == null)
            {
                throw new EntityNotFoundException<PostBookmark>();
            }

            return _mapper.Map<PostBookmarkReadDTO>(postBookmark);
        }
        
        public async Task<IEnumerable<PostReadDTO>> GetOwnPostBookmarkeds(string userId)
        {
            //check User exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Get Post that Bookmarkeds
            var result = await _postBookmarkRepository.FindBookmarkPostsByUserIdAsync(userId);

            return _mapper.Map<List<PostReadDTO>>(result.ToList());
        }
        
        public async Task<PostBookmarkReadDTO> BookmarkAPost(string userId, int postId)
        {
            //check User 
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //check Post
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithIDNotFoundException<Post>(postId);
            }
            //check PostBookmark
            var postBookmark = await _postBookmarkRepository.FindPostBookmarkAsync(postId, userId);
            if (postBookmark == null)
            {
                //postBookmark not existed -> Create new one
                PostBookmark pb = new PostBookmark()
                {
                    PostId = postId,
                    UserId = userId,
                    IsActive = true
                };
                await _postBookmarkRepository.AddAsync(pb);
                await _unitOfWork.CommitAsync();
                
                var returnResult = await _postBookmarkRepository.FindPostBookmarkAsync(postId, userId);
                return _mapper.Map<PostBookmarkReadDTO>(returnResult); 
            }
            else
            {
                //postBookmark already existed -> only change the bool status
                postBookmark.IsActive = !postBookmark.IsActive;
                await _unitOfWork.CommitAsync();
                return _mapper.Map<PostBookmarkReadDTO>(postBookmark); 
            }
        }
    }
}