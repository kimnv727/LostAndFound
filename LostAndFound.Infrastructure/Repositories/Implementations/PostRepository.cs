using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(LostAndFoundDbContext context) : base(context)
        {
        }
        
        public Task<Post> FindPostByIdAsync(int id)
        {
            return _context.Posts.Where(p => p.PostStatus != PostStatus.DELETED).FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<Post> FindPostIncludeDetailsAsync(int id)
        {
            //TODO: Also return comments here
            return _context.Posts.Where(p => p.PostStatus != PostStatus.DELETED).FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<IEnumerable<Post>> FindAllPostsByUserIdAsync(string userId)
        {
            IQueryable<Post> posts = _context.Posts.Where(p => p.PostStatus == PostStatus.ACTIVE);

            posts = posts.Where(p => p.PostUserId == userId);
            
            return await Task.FromResult(posts.ToList());
        }
        
        public async Task<IEnumerable<Post>> QueryPostAsync(PostQuery query, bool trackChanges = false)
        {
            IQueryable<Post> posts = _context.Posts.Where(p => p.PostStatus == PostStatus.ACTIVE).AsSplitQuery();

            if (!trackChanges)
            {
                posts = posts.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.PostUserId))
            {
                posts = posts.Where(p => p.PostUserId == query.PostUserId);
            }

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                posts = posts.Where(p => p.Title.ToLower().Contains(query.Title.ToLower()));
            }
            
            if (!string.IsNullOrWhiteSpace(query.PostContent))
            {
                posts = posts.Where(p => p.PostContent.ToLower().Contains(query.PostContent.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                posts = posts.Where(p => p.Title.ToLower().Contains(query.SearchText.ToLower()) || p.PostContent.ToLower().Contains(query.SearchText.ToLower()));
            }
            
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                posts = posts.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(posts.ToList());
        }
        
        public async Task<IEnumerable<Post>> QueryPostWithStatusAsync(PostQueryWithStatus query, bool trackChanges = false)
        {
            IQueryable<Post> posts = _context.Posts.AsSplitQuery();

            if (!trackChanges)
            {
                posts = posts.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.PostUserId))
            {
                posts = posts.Where(p => p.PostUserId == query.PostUserId);
            }

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                posts = posts.Where(p => p.Title.ToLower().Contains(query.Title.ToLower()));
            }
            
            if (!string.IsNullOrWhiteSpace(query.PostContent))
            {
                posts = posts.Where(p => p.PostContent.ToLower().Contains(query.PostContent.ToLower()));
            }

            if (Enum.IsDefined(query.PostStatus))
            {
                if (query.PostStatus == PostQueryWithStatus.PostStatusQuery.PENDING)
                {
                    posts = posts.Where(p => p.PostStatus == PostStatus.PENDING);
                }
                else if (query.PostStatus == PostQueryWithStatus.PostStatusQuery.CLOSED)
                {
                    posts = posts.Where(p => p.PostStatus == PostStatus.CLOSED);
                }
                else if (query.PostStatus == PostQueryWithStatus.PostStatusQuery.DELETED)
                {
                    posts = posts.Where(p => p.PostStatus == PostStatus.DELETED);
                }
                else if (query.PostStatus == PostQueryWithStatus.PostStatusQuery.ACTIVE)
                {
                    posts = posts.Where(p => p.PostStatus == PostStatus.ACTIVE);
                }
            }
            
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                posts = posts.Where(p => p.Title.ToLower().Contains(query.SearchText.ToLower()) || p.PostContent.ToLower().Contains(query.SearchText.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                posts = posts.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(posts.ToList());
        }
    }
}