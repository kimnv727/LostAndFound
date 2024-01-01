using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IPostRepository :
        IGetAllAsync<Post>,
        IAddAsync<Post>,
        IUpdate<Post>,
        IDelete<Post>
    {
        Task<Post> FindPostByIdAsync(int id);
        Task<IEnumerable<Post>> FindAllPostsByUserIdAsync(string userId);
        Task<Post> FindPostIncludeDetailsAsync(int id);
        Task<IEnumerable<Post>> QueryPostAsync(PostQuery query, bool trackChanges = false);
        Task<IEnumerable<Post>> QueryPostWithStatusAsync(PostQueryWithStatus query, bool trackChanges = false);
        Task<IEnumerable<Post>> QueryPostWithStatusExcludePendingAndRejectedAsync(PostQueryWithStatusExcludePendingAndRejected query, bool trackChanges = false);
        Task<IEnumerable<Post>> QueryPostWithFlagAsync(PostQueryWithFlag query, bool trackChanges = false);
        Task<Post> FindPostByIdAndUserId(int id, string userId);
        public Task UpdatePostRange(Post[] post);
        public Task<IEnumerable<Post>> GetAllActivePosts();
        public Task<IEnumerable<Post>> GetPostsByLocationAndCategoryAsync(int locationId, int categoryId);
        public Task<IEnumerable<Post>> CountNewlyCreatedPost(int month, int year);
        public Task<Dictionary<Category, int>> GetTop10CategoryEntryCountByMonth(int month, int year);
        public Task<IEnumerable<DTOs.Dashboard.Data>> GetPostCountsInDateRanges(int month, int year);
    }
}