using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(LostAndFoundDbContext context) : base(context)
        {
        }
        
        public async Task<Comment> FindCommentByIdAsync(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<Comment> FindCommentWithReplyByIdAsync(int id)
        {
            //TODO: Return reply
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> FindAllCommentsByPostIdAsync(int postId)
        {
            IQueryable<Comment> comments = _context.Comments;

            comments = comments.Where(c => c.PostId == postId).OrderBy(c => c.CreatedDate);
            
            return await Task.FromResult(comments.ToList());
        }
        
        public async Task<IEnumerable<Comment>> FindAllCommentsByUserIdAsync(string userId)
        {
            IQueryable<Comment> comments = _context.Comments;

            comments = comments.Where(c => c.CommentUserId == userId).OrderBy(c => c.CreatedDate);
            
            return await Task.FromResult(comments.ToList());
        }
        
        /*public async Task<IEnumerable<Comment>> FindAllCommentsReplyToCommentId(int commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            
            IQueryable<Comment> comments = _context.Comments;

            comments = comments.Where(c => c.CommentPath.Trim().StartsWith(comment.CommentPath.Trim())).OrderBy(c => c.CreatedDate);
            
            return await Task.FromResult(comments.ToList());
        }*/
        
        public async Task<IEnumerable<Comment>> QueryCommentAsync(CommentQuery query, bool trackChanges = false)
        {
            //TODO: query comment by date?
            IQueryable<Comment> comments = _context.Comments.Where(c => c.CommentStatus == true).AsSplitQuery();

            if (!trackChanges)
            {
                comments = comments.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.CommentUserId))
            {
                comments = comments.Where(c => c.CommentUserId == query.CommentUserId);
            }
            
            if (query.PostId >= 0)
            {
                comments = comments.Where(c => c.PostId == query.PostId);
            }

            if (!string.IsNullOrWhiteSpace(query.CommentContent))
            {
                comments = comments.Where(c => c.CommentContent.ToLower().Contains(query.CommentContent.ToLower()));
            }
            
            if (!string.IsNullOrWhiteSpace(query.CommentPath))
            {
                comments = comments.Where(c => c.CommentPath.Trim().StartsWith(query.CommentPath.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                comments = comments.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(comments.ToList());
        }
        
        public async Task<IEnumerable<Comment>> QueryCommentIgnoreStatusAsync(CommentQuery query, bool trackChanges = false)
        {
            //TODO: query comment by date?
            IQueryable<Comment> comments = _context.Comments.AsSplitQuery();

            if (!trackChanges)
            {
                comments = comments.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.CommentUserId))
            {
                comments = comments.Where(c => c.CommentUserId == query.CommentUserId);
            }
            
            if (query.PostId >= 0)
            {
                comments = comments.Where(c => c.PostId == query.PostId);
            }

            if (!string.IsNullOrWhiteSpace(query.CommentContent))
            {
                comments = comments.Where(c => c.CommentContent.ToLower().Contains(query.CommentContent.ToLower()));
            }
            
            if (!string.IsNullOrWhiteSpace(query.CommentPath))
            {
                comments = comments.Where(c => c.CommentPath.Trim().StartsWith(query.CommentPath.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                comments = comments.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(comments.ToList());
        }
    }
}