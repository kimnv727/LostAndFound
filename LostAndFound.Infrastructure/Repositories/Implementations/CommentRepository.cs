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
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.DeletedDate == null).FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<Comment> FindCommentIgnoreStatusByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        
        /*public async Task<Comment> FindCommentWithReplyByIdAsync(int id)
        {
            //Still get deleted one
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }*/

        public async Task<IEnumerable<Comment>> FindAllCommentsByPostIdAsync(int postId)
        {
            //Still get deleted one
            IQueryable<Comment> comments = _context.Comments;

            comments = comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId).OrderBy(c => c.CreatedDate);
            
            return await Task.FromResult(comments.ToList());
        }
        
        public async Task<IEnumerable<Comment>> FindAllCommentsByUserIdAsync(string userId)
        {
            //Still get deleted one
            IQueryable<Comment> comments = _context.Comments;

            comments = comments
                .Include(c => c.User)
                .Where(c => c.CommentUserId == userId).OrderBy(c => c.CreatedDate);
            
            return await Task.FromResult(comments.ToList());
        }

        public async Task<IEnumerable<Comment>> QueryCommentAsync(CommentQuery query, bool trackChanges = false)
        {
            /*IQueryable<Comment> comments = _context.Comments
                .Include(c => c.User)
                .Where(c => c.DeletedDate == null).AsSplitQuery().AsTracking();*/

            IQueryable<Comment> comments = _context.Comments;

            comments = comments
                .Include(c => c.User);

            if (!trackChanges)
            {
                comments = comments.AsTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.CommentUserId))
            {
                comments = comments.Where(c => c.CommentUserId == query.CommentUserId);
            }
            
            if (query.PostId > 0)
            {
                comments = comments.Where(c => c.PostId == query.PostId);
            }

            if (!string.IsNullOrWhiteSpace(query.CommentContent))
            {
                comments = comments.Where(c => c.CommentContent.ToLower().Contains(query.CommentContent.ToLower()));
            }
            
            if (!string.IsNullOrWhiteSpace(query.CommentPath))
            {
                comments = comments.Where(c => c.CommentPath.Trim().Contains(query.CommentPath.Trim()));
            }

            if (query.FromDate != null)
            {
                comments = comments.Where(p => p.CreatedDate >= query.FromDate);
            }
            
            if (query.ToDate != null)
            {
                comments = comments.Where(p => p.CreatedDate <= query.ToDate);
            }
            
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                comments = comments.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(comments.ToList());
        }
        
        public async Task<IEnumerable<Comment>> QueryCommentIgnoreStatusAsync(CommentQuery query, bool trackChanges = false)
        {
            IQueryable<Comment> comments = _context.Comments
                .Include(c => c.User)
                .AsSplitQuery();

            if (!trackChanges)
            {
                comments = comments.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.CommentUserId))
            {
                comments = comments.Where(c => c.CommentUserId == query.CommentUserId);
            }
            
            if (query.PostId > 0)
            {
                comments = comments.Where(c => c.PostId == query.PostId);
            }

            if (!string.IsNullOrWhiteSpace(query.CommentContent))
            {
                comments = comments.Where(c => c.CommentContent.ToLower().Contains(query.CommentContent.ToLower()));
            }
            
            if (!string.IsNullOrWhiteSpace(query.CommentPath))
            {
                comments = comments.Where(c => c.CommentPath.Trim().Contains(query.CommentPath.Trim()));
            }
            
            if (query.FromDate != null)
            {
                comments = comments.Where(p => p.CreatedDate >= query.FromDate);
            }
            
            if (query.ToDate != null)
            {
                comments = comments.Where(p => p.CreatedDate <= query.ToDate);
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                comments = comments.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(comments.ToList());
        }
    }
}