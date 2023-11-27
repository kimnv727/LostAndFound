using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Exceptions.Post;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class CommentSerivce : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IEmailSendingService _emailSendingService;

        public CommentSerivce(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository, 
            IPasswordHasherService passwordHasherService, IEmailSendingService emailSendingService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _passwordHasherService = passwordHasherService;
            _emailSendingService = emailSendingService;
        }
        
        /*public async Task UpdateCommentStatusAsync(int commentId)
        {
            var comment = await _commentRepository.FindCommentIgnoreStatusByIdAsync(commentId);
            if (comment == null)
            {
                throw new EntityWithIDNotFoundException<Comment>(commentId);
            }

            if (comment.IsActive == true)
            {
                comment.IsActive = false;
            }
            else
            {
                comment.IsActive = true;
            }
            await _unitOfWork.CommitAsync();
        }*/

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _commentRepository.FindCommentByIdAsync(commentId);

            if (comment == null)
            {
                throw new EntityWithIDNotFoundException<Comment>(commentId);
            }

            _commentRepository.Delete(comment);
            await _unitOfWork.CommitAsync();
        }

        public async Task<CommentDetailReadDTO> GetCommentByIdAsync(int commentId)
        {
            var comment = await _commentRepository.FindCommentByIdAsync(commentId);

            if (comment == null)
            {
                throw new EntityWithIDNotFoundException<Comment>(commentId);
            }

            return _mapper.Map<CommentDetailReadDTO>(comment);
        }
        
        public async Task<CommentDetailReadDTO> GetCommentIgnoreStatusByIdAsync(int commentId)
        {
            var comment = await _commentRepository.FindCommentIgnoreStatusByIdAsync(commentId);

            if (comment == null)
            {
                throw new EntityWithIDNotFoundException<Comment>(commentId);
            }

            return _mapper.Map<CommentDetailReadDTO>(comment);
        }
        
        /*public async Task<CommentDetailWithReplyDetailReadDTO> GetCommentWithReplyByIdAsync(int commentId)
        {
            var comment = await _commentRepository.FindCommentWithReplyByIdAsync(commentId);

            if (comment == null)
            {
                throw new EntityWithIDNotFoundException<Comment>(commentId);
            }

            return _mapper.Map<CommentDetailWithReplyDetailReadDTO>(comment);
        }*/

        public async Task<PaginatedResponse<CommentReadDTO>> GetAllCommentByPostIdAsync(int postId)
        {
            //Get Post
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithIDNotFoundException<Post>(postId);
            }
            //Get Comments
            var comments = await _commentRepository.FindAllCommentsByPostIdAsync(postId);

            return _mapper.Map<PaginatedResponse<CommentReadDTO>>(comments);
        }

        public async Task<PaginatedResponse<CommentReadDTO>> QueryCommentAsync(CommentQuery query)
        {
            var comments = await _commentRepository.QueryCommentAsync(query);
            
            return PaginatedResponse<CommentReadDTO>.FromEnumerableWithMapping(comments, query, _mapper);
        }

        public async Task<PaginatedResponse<CommentReadDTO>> QueryCommentIgnoreStatusAsync(CommentQuery query)
        {
            var comments = await _commentRepository.QueryCommentIgnoreStatusAsync(query);
            
            return PaginatedResponse<CommentReadDTO>.FromEnumerableWithMapping(comments, query, _mapper);
        }

        public async Task<CommentReadDTO> CreateCommentAsync(string userId, int postId, CommentWriteDTO commentWriteDTO)
        {
            //Get User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Get Post
            var post = await _postRepository.FindPostByIdAsync(postId);
            if (post == null)
            {
                throw new EntityWithIDNotFoundException<Post>(postId);
            }

            //check Post active
            if (post.PostStatus != PostStatus.ACTIVE)
            {
                throw new PostNotActiveException();
            }

            //Map Comment 
            var comment = _mapper.Map<Comment>(commentWriteDTO);
            comment.IsActive = true;
            comment.PostId = postId;
            comment.CommentUserId = userId;
            //Create Comment
            await _commentRepository.AddAsync(comment);
            await _unitOfWork.CommitAsync();
            var commentReadDTO = _mapper.Map<CommentReadDTO>(comment);
            return commentReadDTO;
        }
        
        public async Task<CommentReadDTO> ReplyToCommentAsync(string userId, int commentId, CommentWriteDTO commentWriteDTO)
        {
            //Get User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Get Comment
            var comment = await _commentRepository.FindCommentByIdAsync(commentId);
            if (comment == null)
            {
                throw new EntityWithIDNotFoundException<Comment>(commentId);
            }
            //Map Reply Comment 
            var replyComment = _mapper.Map<Comment>(commentWriteDTO);
            replyComment.IsActive = true;
            replyComment.PostId = comment.PostId;
            replyComment.CommentUserId = userId;
            replyComment.CommentPath = comment.CommentPath + "/" + comment.Id;
            //Create Comment
            await _commentRepository.AddAsync(replyComment);
            await _unitOfWork.CommitAsync();
            
            var commentReadDTO = _mapper.Map<CommentReadDTO>(replyComment);
            return commentReadDTO;
        }

        public async Task<CommentReadDTO> UpdateCommentDetailsAsync(int commentId, CommentUpdateDTO commentUpdateDTO)
        {
            var comment = await _commentRepository.FindCommentByIdAsync(commentId);
            if (comment == null)
            {
                throw new EntityWithIDNotFoundException<Comment>(commentId);
            }

            _mapper.Map(commentUpdateDTO, comment);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CommentReadDTO>(comment);
        }

        public async Task<bool> CheckCommentAuthorAsync(int commentId, string userId)
        {
            var comment = await _commentRepository.FindCommentByIdAsync(commentId);

            if (comment == null)
                throw new EntityWithIDNotFoundException<Comment>(commentId);

            return comment.CommentUserId == userId ? true : false;
        }

        public async Task<PaginatedResponse<CommentDetailReadWithFlagDTO>> QueryCommentWithFlagAsync(CommentQueryWithFlag query)
        {
            var comments = await _commentRepository.QueryCommentWithFlagAsync(query);
            var result = new List<CommentDetailReadWithFlagDTO>();

            foreach (var c in comments)
            {
                var r = _mapper.Map<CommentDetailReadWithFlagDTO>(c);
                r.WrongInformationCount = c.CommentFlags.Where(f => f.CommentFlagReason == CommentFlagReason.WrongInformation && f.IsActive == true).Count();
                r.SpamCount = c.CommentFlags.Where(f => f.CommentFlagReason == CommentFlagReason.Spam && f.IsActive == true).Count();
                r.ViolatedUserCount = c.CommentFlags.Where(f => f.CommentFlagReason == CommentFlagReason.ViolatedUser && f.IsActive == true).Count();
                r.OthersCount = c.CommentFlags.Where(f => f.CommentFlagReason == CommentFlagReason.Others && f.IsActive == true).Count();
                r.TotalCount = c.CommentFlags.Where(f => f.IsActive == true).Count();
                result.Add(r);
            }

            return PaginatedResponse<CommentDetailReadWithFlagDTO>.FromEnumerableWithMapping(result, query, _mapper);
        }
    }
}