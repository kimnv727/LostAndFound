using System;
using System.ComponentModel;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostQueryWithStatusExcludePendingAndRejected : PaginatedQuery, ISearchTextQuery, IOrderedQuery
    {
        public string PostUserId { get; set; }

        public string Title { get; set; }

        public string PostContent { get; set; }

        public int PostLocationId { get; set; }
        public int PostLocationFloor { get; set; }
        public int?[] PostCategoryId { get; set; }
        public int PostCategoryGroupId { get; set; }

        public enum PostStatusQuery
        {
            All,
            ACTIVE,
            DELETED,
            CLOSED
        }
        [DefaultValue(PostStatusQuery.All)]
        public PostStatusQuery PostStatus { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SearchText { get; set; }
        public string OrderBy { get; set; } = "CreatedDate Desc";
    }
}