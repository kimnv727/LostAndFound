using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.DTOs.PostBookmark;
using LostAndFound.Infrastructure.DTOs.PostFlag;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class PostFlagService : IPostFlagService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPostFlagRepository _postFlagRepository;

        public PostFlagService(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository,
            IPostRepository postRepository, IPostFlagRepository postFlagRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _postFlagRepository = postFlagRepository;
        }

        /*public async Task<int> CountPostFlagAsync(int postId)
        {
            //check Post
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithIDNotFoundException<Post>(postId);
            }
            //count
            var result = await _postFlagRepository.CountPostFlagAsync(postId);

            return result;
        }*/

        public async Task<PostFlagCountReadDTO> CountPostFlagAsync(int postId)
        {
            //check Post
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithIDNotFoundException<Post>(postId);
            }
            //get post flag
            var result = await _postFlagRepository.CountPostFlagAsync(postId);
            //map result
            var response = new PostFlagCountReadDTO()
            {
                FalseInformationCount = 0,
                SpamCount = 0,
                ViolatedUserPoliciesCount = 0,
                TotalCount = 0
            };
            //Count
            response.FalseInformationCount = result.Where(r => r.PostFlagReason == PostFlagReason.FALSE_INFORMATION).Count();
            response.SpamCount = result.Where(r => r.PostFlagReason == PostFlagReason.SPAM).Count();
            response.ViolatedUserPoliciesCount = result.Where(r => r.PostFlagReason == PostFlagReason.VIOLATED_USER_POLICIES).Count();
            //response.OthersCount = result.Where(r => r.PostFlagReason == PostFlagReason.Others).Count();
            response.TotalCount = result.Count();
            /*foreach (var flag in result)
            {
                switch (flag.PostFlagReason)
                {
                    case PostFlagReason.WrongInformation:
                        response.WrongInformationCount++;
                        response.TotalCount++;
                        break;
                    case PostFlagReason.Spam:
                        response.SpamCount++;
                        response.TotalCount++;
                        break;
                    case PostFlagReason.ViolatedUser:
                        response.ViolatedUserCount++;
                        response.TotalCount++;
                        break;
                    case PostFlagReason.Others:
                        response.OthersCount++;
                        response.TotalCount++;
                        break;
                    default:
                        response.TotalCount++;
                        break;
                }
            }*/

            return response;
        }

        public async Task<PostFlagReadDTO> GetPostFlag(string userId, int postId)
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
            
            var postFlag = await _postFlagRepository.FindPostFlagAsync(postId, userId);

            if (postFlag == null)
            {
                throw new EntityNotFoundException<PostFlag>();
            }

            return _mapper.Map<PostFlagReadDTO>(postFlag);
        }
        
        public async Task<IEnumerable<PostReadDTO>> GetOwnPostFlags(string userId)
        {
            //check User exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Get Post that Flagged
            var result = await _postFlagRepository.FindPostFlagsByUserIdAsync(userId);

            return _mapper.Map<List<PostReadDTO>>(result.ToList());
        }
        
        public async Task<PostFlagReadDTO> FlagAPost(string userId, int postId, PostFlagReason reason)
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
            //check PostFlag
            var postFlag = await _postFlagRepository.FindPostFlagAsync(postId, userId);
            if (postFlag == null)
            {
                //postFlag not existed -> Create new one
                PostFlag pf = new PostFlag()
                {
                    PostId = postId,
                    UserId = userId,
                    IsActive = true,
                    PostFlagReason = reason
                };
                await _postFlagRepository.AddAsync(pf);
                await _unitOfWork.CommitAsync();
                
                var returnResult = await _postFlagRepository.FindPostFlagAsync(postId, userId);
                return _mapper.Map<PostFlagReadDTO>(returnResult); 
            }
            else
            {
                //postFlag already existed -> only change the bool status
                //if from true -> false
                if (postFlag.IsActive)
                {
                    postFlag.IsActive = false;
                }
                else if (postFlag.IsActive == false)
                {
                    //if from false -> true
                    postFlag.IsActive = true;
                    postFlag.PostFlagReason = reason;
                }
                await _unitOfWork.CommitAsync();
                return _mapper.Map<PostFlagReadDTO>(postFlag); 
            }
        }
    }
}