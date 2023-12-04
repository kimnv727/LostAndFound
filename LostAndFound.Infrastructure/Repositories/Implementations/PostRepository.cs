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
                //.Include(p => p.Category)
                //.Include(p => p.Location)
                //.ThenInclude(l => l.Property)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post> FindPostIncludeDetailsAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Campus)
                //.Include(p => p.Category)
                //.Include(p => p.Location)
                //.ThenInclude(l => l.Property)
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
                //.Include(p => p.Category)
                //.Include(p => p.Location)
                //.ThenInclude(l => l.Property)
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
                //.Include(p => p.Category)
                //.Include(p => p.Location)
                //.ThenInclude(l => l.Property)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .Where(p => p.PostStatus == PostStatus.ACTIVE).AsSplitQuery();

            if (!trackChanges)
            {
                queriedPosts = queriedPosts.AsNoTracking();
            }

            //Query Posts with Category and Location first
            IQueryable<Post> posts = null;
            if (query.PostCategory != null || query.PostLocation != null)
            {
                //Query Category First
                if (query.PostCategory != null)
                {
                    foreach (var pc in query.PostCategory)
                    {
                        var result = queriedPosts.Where(p => p.PostCategory.Contains("|" + pc + "|"));
                        if (result != null)
                        {
                            posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                        }
                    }
                }
                //Query Location second
                if (query.PostLocation != null)
                {
                    foreach (var pl in query.PostLocation)
                    {
                        var result = queriedPosts.Where(p => p.PostLocation.Contains("|" + pl + "|"));
                        if (result != null)
                        {
                            posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                        }
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

            /*if (query.PostCategoryGroupId > 0)
            {
                posts = posts.Where(p => p.Category.CategoryGroupId == query.PostCategoryGroupId);
            }*/

            /*if (query.PostCategoryId > 0)
            {
                posts = posts.Where(p => p.PostCategoryId == query.PostCategoryId);
            }*/

            /*if (query.PostCategoryId != null)
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

            if (query.FromDate != null)
            {
                posts = posts.Where(p => p.CreatedDate >= query.FromDate);
            }
            
            if (query.ToDate != null)
            {
                posts = posts.Where(p => p.CreatedDate <= query.ToDate);
            }*/

            //Fix later
            /*if (query.PostCategoryName != null)
            {
                //posts = posts.Where(p => query.PostCategoryName.Any(val => p.PostCategory.Contains(val)));
                posts = posts.Where(p => query.PostCategoryName.Any(p.PostCategory.Contains));
            }
            if (query.PostLocationName != null)
            {
                //posts = posts.Where(p => query.PostLocationName.Any(val => p.PostLocation.Contains(val)));
                posts = posts.Where(p => query.PostLocationName.Any(p.PostLocation.Contains));
            }*/
            if (!string.IsNullOrWhiteSpace(query.LostDateFrom))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateFrom) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(query.LostDateTo))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateTo) <= 0);
            }
            if (Enum.IsDefined(query.CampusLocation))
            {
                switch (query.CampusLocation)
                {

                    case PostQuery.CampusLocationQuery.ALL:
                        break;
                    case PostQuery.CampusLocationQuery.HO_CHI_MINH:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case PostQuery.CampusLocationQuery.DA_NANG:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case PostQuery.CampusLocationQuery.CAN_THO:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case PostQuery.CampusLocationQuery.HA_NOI:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    default:
                        break;
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
        
        public async Task<IEnumerable<Post>> QueryPostWithStatusAsync(PostQueryWithStatus query, bool trackChanges = false)
        {
            IQueryable<Post> queriedPosts = _context.Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Campus)
                //.Include(p => p.Category)
                //.Include(p => p.Location)
                //.ThenInclude(l => l.Property)
                .Include(p => p.PostMedias.Where(pm => pm.Media.IsActive == true && pm.Media.DeletedDate == null))
                .ThenInclude(pm => pm.Media)
                .AsSplitQuery();

            if (!trackChanges)
            {
                queriedPosts = queriedPosts.AsNoTracking();
            }

            //Query Posts with Category and Location first
            IQueryable<Post> posts = null;
            if(query.PostCategory != null || query.PostLocation != null)
            {
                //Query Category First
                if(query.PostCategory != null)
                {
                    foreach(var pc in query.PostCategory)
                    {
                        var result = queriedPosts.Where(p => p.PostCategory.Contains("|" + pc + "|"));
                        if(result != null)
                        {
                            posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                        }
                    }
                }
                //Query Location second
                if (query.PostLocation != null)
                {
                    foreach (var pl in query.PostLocation)
                    {
                        var result = queriedPosts.Where(p => p.PostLocation.Contains("|" + pl + "|"));
                        if (result != null)
                        {
                            posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                        }
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

            /*if (query.PostCategoryGroupId > 0)
            {
                posts = posts.Where(p => p.Category.CategoryGroupId == query.PostCategoryGroupId);
            }

            *//*if (query.PostCategoryId > 0)
            {
                posts = posts.Where(p => p.PostCategoryId == query.PostCategoryId);
            }*//*

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
            }*/
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

            /*if (query.FromDate != null)
            {
                posts = posts.Where(p => p.CreatedDate >= query.FromDate);
            }
            
            if (query.ToDate != null)
            {
                posts = posts.Where(p => p.CreatedDate <= query.ToDate);
            }*/

            //Fix later
            /*if (query.PostCategoryName != null)
            {
                //posts = posts.Where(p => query.PostCategoryName.Any(val => p.PostCategory.Contains(val)));
                posts = posts.Where(p => query.PostCategoryName.Any(p.PostCategory.Contains));
            }
            if (query.PostLocationName != null)
            {
                //posts = posts.Where(p => query.PostLocationName.Any(val => p.PostLocation.Contains(val)));
                posts = posts.Where(p => query.PostLocationName.Any(p.PostLocation.Contains));
            }*/
            if (!string.IsNullOrWhiteSpace(query.LostDateFrom))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateFrom) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(query.LostDateTo))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateTo) <= 0);
            }
            if (Enum.IsDefined(query.CampusLocation))
            {
                switch (query.CampusLocation)
                {

                    case PostQueryWithStatus.CampusLocationQuery.ALL:
                        break;
                    case PostQueryWithStatus.CampusLocationQuery.HO_CHI_MINH:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case PostQueryWithStatus.CampusLocationQuery.DA_NANG:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case PostQueryWithStatus.CampusLocationQuery.CAN_THO:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case PostQueryWithStatus.CampusLocationQuery.HA_NOI:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    default:
                        break;
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

        public async Task<IEnumerable<Post>> QueryPostWithStatusExcludePendingAndRejectedAsync(PostQueryWithStatusExcludePendingAndRejected query, bool trackChanges = false)
        {
            IQueryable<Post> queriedPosts = _context.Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Campus)
                //.Include(p => p.Category)
                //.Include(p => p.Location)
                //.ThenInclude(l => l.Property)
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
            if (query.PostCategory != null || query.PostLocation != null)
            {
                //Query Category First
                if (query.PostCategory != null)
                {
                    foreach (var pc in query.PostCategory)
                    {
                        var result = queriedPosts.Where(p => p.PostCategory.Contains("|" + pc + "|"));
                        if (result != null)
                        {
                            posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                        }
                    }
                }
                //Query Location second
                if (query.PostLocation != null)
                {
                    foreach (var pl in query.PostLocation)
                    {
                        var result = queriedPosts.Where(p => p.PostLocation.Contains("|" + pl + "|"));
                        if (result != null)
                        {
                            posts = posts == null ? posts = result : posts.Concat(result).Distinct();
                        }
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

            /*if (query.PostCategoryGroupId > 0)
            {
                posts = posts.Where(p => p.Category.CategoryGroupId == query.PostCategoryGroupId);
            }

            *//*if (query.PostCategoryId > 0)
            {
                posts = posts.Where(p => p.PostCategoryId == query.PostCategoryId);
            }*//*

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
            }*/
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

            /*if (query.FromDate != null)
            {
                posts = posts.Where(p => p.CreatedDate >= query.FromDate);
            }

            if (query.ToDate != null)
            {
                posts = posts.Where(p => p.CreatedDate <= query.ToDate);
            }*/

            //Fix later
           /* if (query.PostCategoryName != null)
            {
                //posts = posts.Where(p => query.PostCategoryName.Any(val => p.PostCategory.Contains(val)));
                posts = posts.Where(p => query.PostCategoryName.Any(p.PostCategory.Contains));
            }
            if (query.PostLocationName != null)
            {
                //posts = posts.Where(p => query.PostLocationName.Any(val => p.PostLocation.Contains(val)));
                posts = posts.Where(p => query.PostLocationName.Any(p.PostLocation.Contains));
            }*/
            if (!string.IsNullOrWhiteSpace(query.LostDateFrom))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateFrom) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(query.LostDateTo))
            {
                posts = posts.Where(p => p.LostDateFrom.CompareTo(query.LostDateTo) <= 0);
            }
            if (Enum.IsDefined(query.CampusLocation))
            {
                switch (query.CampusLocation)
                {

                    case PostQueryWithStatusExcludePendingAndRejected.CampusLocationQuery.ALL:
                        break;
                    case PostQueryWithStatusExcludePendingAndRejected.CampusLocationQuery.HO_CHI_MINH:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case PostQueryWithStatusExcludePendingAndRejected.CampusLocationQuery.DA_NANG:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case PostQueryWithStatusExcludePendingAndRejected.CampusLocationQuery.CAN_THO:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case PostQueryWithStatusExcludePendingAndRejected.CampusLocationQuery.HA_NOI:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    default:
                        break;
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

        public async Task<IEnumerable<Post>> QueryPostWithFlagAsync(PostQueryWithFlag query, bool trackChanges = false)
        {
            IQueryable<Post> posts = _context.Posts
                .Include(p => p.User)
                .ThenInclude(u => u.Campus)
                //.Include(p => p.Category)
                //.Include(p => p.Location)
                //.ThenInclude(l => l.Property)
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

            /*if (query.PostCategoryGroupId > 0)
            {
                posts = posts.Where(p => p.Category.CategoryGroupId == query.PostCategoryGroupId);
            }

            *//*if (query.PostCategoryId > 0)
            {
                posts = posts.Where(p => p.PostCategoryId == query.PostCategoryId);
            }*//*

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
            }*/
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

            /*if (query.FromDate != null)
            {
                posts = posts.Where(p => p.CreatedDate >= query.FromDate);
            }

            if (query.ToDate != null)
            {
                posts = posts.Where(p => p.CreatedDate <= query.ToDate);
            }*/
            //Fix later
            /*if (query.PostCategoryName != null)
            {
                //posts = posts.Where(p => query.PostCategoryName.Any(val => p.PostCategory.Contains(val)));
                posts = posts.Where(p => query.PostCategoryName.Any(p.PostCategory.Contains));
            }
            if (query.PostLocationName != null)
            {
                //posts = posts.Where(p => query.PostLocationName.Any(val => p.PostLocation.Contains(val)));
                posts = posts.Where(p => query.PostLocationName.Any(p.PostLocation.Contains));
            }*/
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
            if (Enum.IsDefined(query.CampusLocation))
            {
                switch (query.CampusLocation)
                {

                    case PostQueryWithFlag.CampusLocationQuery.ALL:
                        break;
                    case PostQueryWithFlag.CampusLocationQuery.HO_CHI_MINH:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case PostQueryWithFlag.CampusLocationQuery.DA_NANG:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case PostQueryWithFlag.CampusLocationQuery.CAN_THO:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    case PostQueryWithFlag.CampusLocationQuery.HA_NOI:
                        posts = posts.Where(p => p.User.Campus.CampusLocation == CampusLocation.HO_CHI_MINH);
                        break;
                    default:
                        break;
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
                .ThenInclude(u => u.Campus)
                //.Include(p => p.Category)
                //.Include(p => p.Location)
                //.ThenInclude(l => l.Property)
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
    }
}