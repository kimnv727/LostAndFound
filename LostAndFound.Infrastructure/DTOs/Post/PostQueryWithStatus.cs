using System.ComponentModel;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostQueryWithStatus : PaginatedQuery, ISearchTextQuery, IOrderedQuery 
    {
        public string PostUserId { get; set; }

        public string Title { get; set; }

        public string PostContent { get; set; }
        
        //TODO: Query By CreatedDate?
        public enum PostStatusQuery
        {
            All,
            PENDING,
            ACTIVE,
            DELETED,
            CLOSED
        }
        [DefaultValue(PostStatusQuery.All)]
        public PostStatusQuery PostStatus { get; set; }

        public string SearchText { get; set; }

        public string OrderBy { get; set; } = "CreatedDate Desc";
    }
}