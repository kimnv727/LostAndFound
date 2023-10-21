using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.DTOs.CommentFlag;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.DTOs.PostFlag;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class CommentFlagService : ICommentFlagService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentFlagRepository _commentFlagRepository;

        public CommentFlagService(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository,
            ICommentRepository commentRepository, ICommentFlagRepository commentFlagRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _commentFlagRepository = commentFlagRepository;
        }

        /*public async Task<int> CountCommentFlagAsync(int commentId)
        {
            //check Post
            var comment = await _commentRepository.FindCommentByIdAsync(commentId);
            if (comment == null)
            {
                throw new EntityWithIDNotFoundException<Comment>(commentId);
            }
            //count
            var result = await _commentFlagRepository.CountCommentFlagAsync(commentId);

            return result;
        }*/

        public async Task<CommentFlagCountReadDTO> CountCommentFlagAsync(int commentId)
        {
            //check Post
            var comment = await _commentRepository.FindCommentByIdAsync(commentId);
            if (comment == null)
            {
                throw new EntityWithIDNotFoundException<Comment>(commentId);
            }
            //get comment flag
            var result = await _commentFlagRepository.CountCommentFlagAsync(commentId);
            //map result
            var response = new CommentFlagCountReadDTO()
            {
                WrongInformationCount = 0,
                SpamCount = 0,
                ViolatedUserCount = 0,
                OthersCount = 0,
                TotalCount = 0
            };
            //Count
            foreach (var flag in result)
            {
                switch (flag.CommentFlagReason)
                {
                    case CommentFlagReason.WrongInformation:
                        response.WrongInformationCount++;
                        response.TotalCount++;
                        break;
                    case CommentFlagReason.Spam:
                        response.SpamCount++;
                        response.TotalCount++;
                        break;
                    case CommentFlagReason.ViolatedUser:
                        response.ViolatedUserCount++;
                        response.TotalCount++;
                        break;
                    case CommentFlagReason.Others:
                        response.OthersCount++;
                        response.TotalCount++;
                        break;
                    default:
                        response.TotalCount++;
                        break;
                }
            }

            return response;
        }

        public async Task<CommentFlagReadDTO> GetCommentFlag(string userId, int commentId)
        {
            //check User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //check Comment
            var comment = await _commentRepository.FindCommentByIdAsync(commentId);
            if (comment == null)
            {
                throw new EntityWithIDNotFoundException<Comment>(commentId);
            }
            
            var commentFlag = await _commentFlagRepository.FindCommentFlagAsync(commentId, userId);

            if (commentFlag == null)
            {
                throw new EntityNotFoundException<CommentFlag>();
            }

            return _mapper.Map<CommentFlagReadDTO>(commentFlag);
        }
        
        public async Task<IEnumerable<CommentReadDTO>> GetOwnCommentFlags(string userId)
        {
            //check User exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Get Comment that Flagged
            var result = await _commentFlagRepository.FindCommentFlagsByUserIdAsync(userId);

            return _mapper.Map<List<CommentReadDTO>>(result.ToList());
        }
        
        public async Task<CommentFlagReadDTO> FlagAComment(string userId, int commentId, CommentFlagReason reason)
        {
            //check User 
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //check Comment
            var comment = await _commentRepository.FindCommentByIdAsync(commentId);
            if (comment == null)
            {
                throw new EntityWithIDNotFoundException<Comment>(commentId);
            }
            //check CommentFlag
            var commentFlag = await _commentFlagRepository.FindCommentFlagAsync(commentId, userId);
            if (commentFlag == null)
            {
                //commentflag not existed -> Create new one
                CommentFlag cf = new CommentFlag()
                {
                    CommentId = commentId,
                    UserId = userId,
                    IsActive = true,
                    CommentFlagReason = reason
                };
                await _commentFlagRepository.AddAsync(cf);
                await _unitOfWork.CommitAsync();
                
                var returnResult = await _commentFlagRepository.FindCommentFlagAsync(commentId, userId);
                return _mapper.Map<CommentFlagReadDTO>(returnResult); 
            }
            else
            {
                //commentFlag already existed -> only change the bool status
                //if from true -> false
                if (commentFlag.IsActive)
                {
                    commentFlag.IsActive = false;
                }
                else if (commentFlag.IsActive == false)
                {
                    //if from false -> true
                    commentFlag.IsActive = true;
                    commentFlag.CommentFlagReason = reason;
                }
                await _unitOfWork.CommitAsync();
                return _mapper.Map<CommentFlagReadDTO>(commentFlag); 
            }
        }
    }
}