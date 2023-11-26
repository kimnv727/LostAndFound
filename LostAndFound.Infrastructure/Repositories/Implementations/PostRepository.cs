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
        //TODO: should we make item closed to outdated appear on top?
        public async Task<Post> FindPostByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post> FindPostIncludeDetailsAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Include(p => p.Comments)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<IEnumerable<Post>> FindAllPostsByUserIdAsync(string userId)
        {
            IQueryable<Post> posts = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .Where(p => p.PostStatus == PostStatus.ACTIVE);

            posts = posts.Where(p => p.PostUserId == userId);
            
            return await Task.FromResult(posts.ToList());
        }
        
        public async Task<IEnumerable<Post>> QueryPostAsync(PostQuery query, bool trackChanges = false)
        {
            IQueryable<Post> posts = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .Where(p => p.PostStatus == PostStatus.ACTIVE).AsSplitQuery();

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

            if (query.PostCategoryGroupId > 0)
            {
                posts = posts.Where(p => p.Category.CategoryGroupId == query.PostCategoryGroupId);
            }
            
            if (query.PostCategoryId > 0)
            {
                posts = posts.Where(p => p.PostCategoryId == query.PostCategoryId);
            }
            
            if (query.PostLocationId > 0)
            {
                posts = posts.Where(p => p.PostLocationId == query.PostLocationId);
            }
            if (query.PostLocationFloor >= 0)
            {
                posts = posts.Where(p => p.Location.Floor == query.PostLocationFloor);
            }

            if (query.FromDate != null)
            {
                posts = posts.Where(p => p.CreatedDate >= query.FromDate);
            }
            
            if (query.ToDate != null)
            {
                posts = posts.Where(p => p.CreatedDate <= query.ToDate);
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
            IQueryable<Post> posts = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .AsSplitQuery();

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

            if (query.PostCategoryGroupId > 0)
            {
                posts = posts.Where(p => p.Category.CategoryGroupId == query.PostCategoryGroupId);
            }

            /*if (query.PostCategoryId > 0)
            {
                posts = posts.Where(p => p.PostCategoryId == query.PostCategoryId);
            }*/

            if (query.PostCategoryId != null)
            {
                posts = posts.Where(p => query.PostCategoryId.Contains(p.PostCategoryId));
            }

            if (query.PostLocationId > 0)
            {
                posts = posts.Where(p => p.PostLocationId == query.PostLocationId);
            }
            if (query.PostLocationFloor >= 0)
            {
                posts = posts.Where(p => p.Location.Floor == query.PostLocationFloor);
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
                else if (query.PostStatus == PostQueryWithStatus.PostStatusQuery.REJECTED)
                {
                    posts = posts.Where(p => p.PostStatus == PostStatus.REJECTED);
                }
            }
            
            if (query.FromDate != null)
            {
                posts = posts.Where(p => p.CreatedDate >= query.FromDate);
            }
            
            if (query.ToDate != null)
            {
                posts = posts.Where(p => p.CreatedDate <= query.ToDate);
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

        public async Task<IEnumerable<Post>> QueryPostWithStatusExcludePendingAndRejectedAsync(PostQueryWithStatusExcludePendingAndRejected query, bool trackChanges = false)
        {
            IQueryable<Post> posts = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .Where(p => p.PostStatus != PostStatus.PENDING && p.PostStatus != PostStatus.REJECTED)
                .AsSplitQuery();

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

            if (query.PostCategoryGroupId > 0)
            {
                posts = posts.Where(p => p.Category.CategoryGroupId == query.PostCategoryGroupId);
            }

            /*if (query.PostCategoryId > 0)
            {
                posts = posts.Where(p => p.PostCategoryId == query.PostCategoryId);
            }*/

            if (query.PostCategoryId != null)
            {
                posts = posts.Where(p => query.PostCategoryId.Contains(p.PostCategoryId));
            }

            if (query.PostLocationId > 0)
            {
                posts = posts.Where(p => p.PostLocationId == query.PostLocationId);
            }
            if (query.PostLocationFloor >= 0)
            {
                posts = posts.Where(p => p.Location.Floor == query.PostLocationFloor);
            }
            if (Enum.IsDefined(query.PostStatus))
            {
                if (query.PostStatus == PostQueryWithStatusExcludePendingAndRejected.PostStatusQuery.CLOSED)
                {
                    posts = posts.Where(p => p.PostStatus == PostStatus.CLOSED);
                }
                else if (query.PostStatus == PostQueryWithStatusExcludePendingAndRejected.PostStatusQuery.DELETED)
                {
                    posts = posts.Where(p => p.PostStatus == PostStatus.DELETED);
                }
                else if (query.PostStatus == PostQueryWithStatusExcludePendingAndRejected.PostStatusQuery.ACTIVE)
                {
                    posts = posts.Where(p => p.PostStatus == PostStatus.ACTIVE);
                }
            }

            if (query.FromDate != null)
            {
                posts = posts.Where(p => p.CreatedDate >= query.FromDate);
            }

            if (query.ToDate != null)
            {
                posts = posts.Where(p => p.CreatedDate <= query.ToDate);
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

        public async Task<IEnumerable<Post>> QueryPostWithFlagAsync(PostQueryWithFlag query, bool trackChanges = false)
        {
            IQueryable<Post> posts = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Include(p => p.PostFlags)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .Where(p => p.PostStatus != PostStatus.PENDING && p.PostStatus != PostStatus.REJECTED && p.PostFlags.Count() > 0)
                .AsSplitQuery();

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

            if (query.PostCategoryGroupId > 0)
            {
                posts = posts.Where(p => p.Category.CategoryGroupId == query.PostCategoryGroupId);
            }

            /*if (query.PostCategoryId > 0)
            {
                posts = posts.Where(p => p.PostCategoryId == query.PostCategoryId);
            }*/

            if (query.PostCategoryId != null)
            {
                posts = posts.Where(p => query.PostCategoryId.Contains(p.PostCategoryId));
            }

            if (query.PostLocationId > 0)
            {
                posts = posts.Where(p => p.PostLocationId == query.PostLocationId);
            }
            if (query.PostLocationFloor >= 0)
            {
                posts = posts.Where(p => p.Location.Floor == query.PostLocationFloor);
            }
            if (Enum.IsDefined(query.PostStatus))
            {
                if (query.PostStatus == PostQueryWithFlag.PostStatusQuery.CLOSED)
                {
                    posts = posts.Where(p => p.PostStatus == PostStatus.CLOSED);
                }
                else if (query.PostStatus == PostQueryWithFlag.PostStatusQuery.DELETED)
                {
                    posts = posts.Where(p => p.PostStatus == PostStatus.DELETED);
                }
                else if (query.PostStatus == PostQueryWithFlag.PostStatusQuery.ACTIVE)
                {
                    posts = posts.Where(p => p.PostStatus == PostStatus.ACTIVE);
                }
            }

            if (query.FromDate != null)
            {
                posts = posts.Where(p => p.CreatedDate >= query.FromDate);
            }

            if (query.ToDate != null)
            {
                posts = posts.Where(p => p.CreatedDate <= query.ToDate);
            }

            if (query.FlagCount > 0)
            {
                posts = posts.Where(p => p.PostFlags.Count() >= query.FlagCount);
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

        public async Task<Post> FindPostByIdAndUserId(int id, string userId)
        {
            return await _context.Posts
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .SingleOrDefaultAsync(p => p.Id == id && p.PostUserId == userId);
        }

        public async Task UpdatePostRange(Post[] posts)
        {
            _context.Posts.UpdateRange(posts);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> GetAllActivePosts()
        {
            var posts = _context.Posts
                .Where(p => p.PostStatus == PostStatus.ACTIVE);

            return await Task.FromResult(posts.ToList());
        }

        public async Task<IEnumerable<Post>> GetPostsByLocationAndCategoryAsync(int locationId, int categoryId)
        {
            var posts = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Include(p => p.PostFlags)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .Where(p => p.PostStatus == PostStatus.ACTIVE && p.PostLocationId == locationId && p.PostCategoryId == categoryId)
                .AsSplitQuery();

            posts = posts
                .OrderBy(p => p.CreatedDate);

            posts = posts.AsNoTracking();

            return await Task.FromResult(posts.ToList());
        }
    }
}