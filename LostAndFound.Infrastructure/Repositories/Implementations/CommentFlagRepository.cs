using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class CommentFlagRepository : GenericRepository<CommentFlag>, ICommentFlagRepository
    {
        public CommentFlagRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        /*public async Task<int> CountCommentFlagAsync(int commentId)
        {
            var result = _context.CommentFlags.Where(cf => cf.CommentId == commentId && cf.IsActive == true);
            return await Task.FromResult(result.Count());
        }*/

        public async Task<IEnumerable<CommentFlag>> CountCommentFlagAsync(int commentId)
        {
            var result = _context.CommentFlags.Where(cf => cf.CommentId == commentId && cf.IsActive == true);
            return await Task.FromResult(result.ToList());
        }

        public async Task<IEnumerable<Comment>> FindCommentFlagsByUserIdAsync(string userId)
        {
            IQueryable<CommentFlag> commentFlags = _context.CommentFlags.Where(cf => cf.UserId == userId && cf.IsActive == true);
            IQueryable<Comment> comments = commentFlags.Select(cf => cf.Comment);

            return await Task.FromResult(comments.ToList());
        }

        public async Task<CommentFlag> FindCommentFlagAsync(int commentId, string userId)
        {
            return await _context.CommentFlags.Include(cf => cf.Comment).FirstOrDefaultAsync(cf => cf.CommentId == commentId && cf.UserId == userId);
        }
    }
}