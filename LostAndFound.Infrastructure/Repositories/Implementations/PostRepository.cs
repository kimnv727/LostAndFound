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

        public async Task<Post> FindPostByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Campus)
                .Include(p => p.Categories)
                .Include(p => p.Locations)
                .ThenInclude(l => l.Campus)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post> FindPostIncludeDetailsAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Campus)
                .Include(p => p.Categories)
                .Include(p => p.Locations)
                .ThenInclude(l => l.Campus)
                .Include(p => p.Comments)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<IEnumerable<Post>> FindAllPostsByUserIdAsync(string userId)
        {
            IQueryable<Post> posts = _context.Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Campus)
                .Include(p => p.Categories)
                .Include(p => p.Locations)
                .ThenInclude(l => l.Campus)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .Where(p => p.PostStatus == PostStatus.ACTIVE);

            posts = posts.Where(p => p.PostUserId == userId);
            
            return await Task.FromResult(posts.ToList());
        }
        
        public async Task<IEnumerable<Post>> QueryPostAsync(PostQuery query, bool trackChanges = false)
        {
            IQueryable<Post> queriedPosts = _context.Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Campus)
                .Include(p => p.Categories)
                .Include(p => p.Locations)
                .ThenInclude(l => l.Campus)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .Where(p => p.PostStatus == PostStatus.ACTIVE).AsSplitQuery();

            if (!trackChanges)
            {
                queriedPosts = queriedPosts.AsNoTracking();
            }

            //Query Posts with Category and Location first
            IQueryable<Post> posts = null;
            if (query.PostCategory != null && query.PostLocation != null)
            {
                //Query Category First
                if (query.PostCategory != null)
                {
                    var result = queriedPosts.Where(p => p.Categories.Any(c => query.PostCategory.Contains(c.Id)));
                    if (result != null)
                    {
                        posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                    }
                }
                //Query Location second
                if (query.PostLocation != null)
                {
                    posts = posts.Where(p => p.Locations.Any(l => query.PostLocation.Contains(l.Id)));
                }
            }
            else if (query.PostCategory != null || query.PostLocation != null)
            {
                //Query Category First
                if (query.PostCategory != null)
                {
                    var result = queriedPosts.Where(p => p.Categories.Any(c => query.PostCategory.Contains(c.Id)));
                    if (result != null)
                    {
                        posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                    }
                }
                //Query Location second
                if (query.PostLocation != null)
                {
                    var result = queriedPosts.Where(p => p.Locations.Any(l => query.PostLocation.Contains(l.Id)));
                    if (result != null)
                    {
                        posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                    }
                }
            }
            else
            {
                posts = queriedPosts;
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

            if (!string.IsNullOrWhiteSpace(query.LostDateFrom))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateFrom) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(query.LostDateTo))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateTo) <= 0);
            }

            if (query.CampusId > 0)
            {
                posts = posts.Where(p => p.User.CampusId == query.CampusId);
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
            IQueryable<Post> queriedPosts = _context.Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Campus)
                .Include(p => p.Categories)
                .Include(p => p.Locations)
                .ThenInclude(l => l.Campus)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .AsSplitQuery();

            if (!trackChanges)
            {
                queriedPosts = queriedPosts.AsNoTracking();
            }

            //Query Posts with Category and Location first
            IQueryable<Post> posts = null;
            if(query.PostCategory != null && query.PostLocation != null)
            {
                //Query Category First
                if (query.PostCategory != null)
                {
                    var result = queriedPosts.Where(p => p.Categories.Any(c => query.PostCategory.Contains(c.Id)));
                    if (result != null)
                    {
                        posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                    }
                }
                //Query Location second
                if (query.PostLocation != null)
                {
                    posts = posts.Where(p => p.Locations.Any(l => query.PostLocation.Contains(l.Id)));
                }
            }
            else if (query.PostCategory != null || query.PostLocation != null)
            {
                //Query Category First
                if (query.PostCategory != null)
                {
                    var result = queriedPosts.Where(p => p.Categories.Any(c => query.PostCategory.Contains(c.Id)));
                    if (result != null)
                    {
                        posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                    }
                }
                //Query Location second
                if (query.PostLocation != null)
                {
                    var result = queriedPosts.Where(p => p.Locations.Any(l => query.PostLocation.Contains(l.Id)));
                    if (result != null)
                    {
                        posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                    }
                }
            }
            else
            {
                posts = queriedPosts;
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
                else if (query.PostStatus == PostQueryWithStatus.PostStatusQuery.REJECTED)
                {
                    posts = posts.Where(p => p.PostStatus == PostStatus.REJECTED);
                }
            }

            if (!string.IsNullOrWhiteSpace(query.LostDateFrom))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateFrom) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(query.LostDateTo))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateTo) <= 0);
            }

            if (query.CampusId > 0)
            {
                posts = posts.Where(p => p.User.CampusId == query.CampusId);
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
            IQueryable<Post> queriedPosts = _context.Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Campus)
                .Include(p => p.Categories)
                .Include(p => p.Locations)
                .ThenInclude(l => l.Campus)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .Where(p => p.PostStatus != PostStatus.PENDING && p.PostStatus != PostStatus.REJECTED)
                .AsSplitQuery();

            if (!trackChanges)
            {
                queriedPosts = queriedPosts.AsNoTracking();
            }

            //Query Posts with Category and Location first
            IQueryable<Post> posts = null;
            if (query.PostCategory != null && query.PostLocation != null)
            {
                //Query Category First
                if (query.PostCategory != null)
                {
                    var result = queriedPosts.Where(p => p.Categories.Any(c => query.PostCategory.Contains(c.Id)));
                    if (result != null)
                    {
                        posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                    }
                }
                //Query Location second
                if (query.PostLocation != null)
                {
                    posts = posts.Where(p => p.Locations.Any(l => query.PostLocation.Contains(l.Id)));
                }
            }
            else if (query.PostCategory != null || query.PostLocation != null)
            {
                //Query Category First
                if (query.PostCategory != null)
                {
                    var result = queriedPosts.Where(p => p.Categories.Any(c => query.PostCategory.Contains(c.Id)));
                    if (result != null)
                    {
                        posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                    }
                }
                //Query Location second
                if (query.PostLocation != null)
                {
                    var result = queriedPosts.Where(p => p.Locations.Any(l => query.PostLocation.Contains(l.Id)));
                    if (result != null)
                    {
                        posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                    }
                }
            }
            else
            {
                posts = queriedPosts;
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

            if (!string.IsNullOrWhiteSpace(query.LostDateFrom))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateFrom) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(query.LostDateTo))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateTo) <= 0);
            }

            if (query.CampusId > 0)
            {
                posts = posts.Where(p => p.User.CampusId == query.CampusId);
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
            IQueryable<Post> queriedPosts = _context.Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Campus)
                .Include(p => p.Categories)
                .Include(p => p.Locations)
                .ThenInclude(l => l.Campus)
                .Include(p => p.PostFlags)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .Where(p => p.PostStatus != PostStatus.PENDING && p.PostStatus != PostStatus.REJECTED && p.PostFlags.Count() > 0)
                .AsSplitQuery();

            if (!trackChanges)
            {
                queriedPosts = queriedPosts.AsNoTracking();
            }

            //Query Posts with Category and Location first
            IQueryable<Post> posts = null;
            if (query.PostCategory != null && query.PostLocation != null)
            {
                //Query Category First
                if (query.PostCategory != null)
                {
                    var result = queriedPosts.Where(p => p.Categories.Any(c => query.PostCategory.Contains(c.Id)));
                    if (result != null)
                    {
                        posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                    }
                }
                //Query Location second
                if (query.PostLocation != null)
                {
                    posts = posts.Where(p => p.Locations.Any(l => query.PostLocation.Contains(l.Id)));
                }
            }
            else if (query.PostCategory != null || query.PostLocation != null)
            {
                //Query Category First
                if (query.PostCategory != null)
                {
                    var result = queriedPosts.Where(p => p.Categories.Any(c => query.PostCategory.Contains(c.Id)));
                    if (result != null)
                    {
                        posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                    }
                }
                //Query Location second
                if (query.PostLocation != null)
                {
                    var result = queriedPosts.Where(p => p.Locations.Any(l => query.PostLocation.Contains(l.Id)));
                    if (result != null)
                    {
                        posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                    }
                }
            }
            else
            {
                posts = queriedPosts;
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

            if (!string.IsNullOrWhiteSpace(query.LostDateFrom))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateFrom) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(query.LostDateTo))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateTo) <= 0);
            }

            if (query.FlagCount > 0)
            {
                posts = posts.Where(p => p.PostFlags.Count() >= query.FlagCount);
            }

            if (query.CampusId > 0)
            {
                posts = posts.Where(p => p.User.CampusId == query.CampusId);
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
                .Include(p => p.Categories)
                .Include(p => p.Locations)
                .ThenInclude(l => l.Campus)
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
                .Include(p => p.Categories)
                .Include(p => p.Locations)
                .ThenInclude(l => l.Campus)
                .Where(p => p.PostStatus == PostStatus.ACTIVE);

            return await Task.FromResult(posts.ToList());
        }

        public async Task<IEnumerable<Post>> GetPostsByLocationAndCategoryAsync(int locationId, int categoryId)
        {
            var posts = _context.Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Campus)
                .Include(p => p.Categories)
                .Include(p => p.Locations)
                .ThenInclude(l => l.Campus)
                .Include(p => p.PostFlags)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                //.Where(p => p.PostStatus == PostStatus.ACTIVE && p.PostLocationId == locationId && p.PostCategoryId == categoryId)
                .AsSplitQuery();

            posts = posts
                .OrderBy(p => p.CreatedDate);

            posts = posts.AsNoTracking();

            return await Task.FromResult(posts.ToList());
        }

        public async Task<IEnumerable<Post>> CountNewlyCreatedPost(int month, int year)
        {
            var result = _context.Posts.Where(p => p.CreatedDate.Month == month && p.CreatedDate.Year == year
            && p.PostStatus != PostStatus.REJECTED && p.PostStatus != PostStatus.DELETED && p.PostStatus != PostStatus.PENDING);
            return await Task.FromResult(result.ToList());
        }

        public async Task<Dictionary<Category, int>> GetTop10CategoryEntryCountByMonth(int month, int year)
        {
            var top10CategoryEntryCounts = _context.Categories
                .Include(c => c.CategoryGroup)
                .Select(category => new
                {
                    Category = category,
                    EntryCount = category.Items.Count(i => i.CreatedDate.Month == month && i.CreatedDate.Year == year 
                    && i.ItemStatus != ItemStatus.DELETED && i.ItemStatus != ItemStatus.PENDING && i.ItemStatus != ItemStatus.REJECTED)
                                 + category.Posts.Count(p => p.CreatedDate.Month == month && p.CreatedDate.Year == year
                    && p.PostStatus != PostStatus.DELETED && p.PostStatus != PostStatus.PENDING && p.PostStatus != PostStatus.REJECTED)
                })
                .OrderByDescending(x => x.EntryCount)
                .Take(10)
                .ToDictionary(x => x.Category, x => x.EntryCount);

            return top10CategoryEntryCounts;
        }

        public async Task<IEnumerable<DTOs.Dashboard.Data>> GetPostCountsInDateRanges(int month, int year)
        {
            //get number of days
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var result = new List<DTOs.Dashboard.Data>();

            for (int i = 0; i <= 8; i++)
            {
                var data = new DTOs.Dashboard.Data();
                data.x = (i * 3 + 3).ToString() + "/" + month.ToString() + "/" + year.ToString();
                data.y = _context.Posts.Where(p => (p.CreatedDate.Day >= i*3 + 1 && p.CreatedDate.Day <= i*3 +3)
                && p.CreatedDate.Month == month && p.CreatedDate.Year == year
                && p.PostStatus != PostStatus.REJECTED && p.PostStatus != PostStatus.DELETED && p.PostStatus != PostStatus.PENDING).Count();

                result.Add(data);
            }

            var lastData = new DTOs.Dashboard.Data();
            lastData.x = (daysInMonth).ToString() + "/" + month.ToString() + "/" + year.ToString();
            lastData.y = _context.Posts.Where(p => (p.CreatedDate.Day >= 28 && p.CreatedDate.Day <= daysInMonth)
                && p.CreatedDate.Month == month && p.CreatedDate.Year == year
                && p.PostStatus != PostStatus.REJECTED && p.PostStatus != PostStatus.DELETED && p.PostStatus != PostStatus.PENDING).Count();
            result.Add(lastData);

            return result;
        }
    }
}