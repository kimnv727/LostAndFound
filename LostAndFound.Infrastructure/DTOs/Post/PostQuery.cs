using System;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostQuery : PaginatedQuery, IOrderedQuery 
    {
        public string PostUserId { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SearchText { get; set; }
        public string OrderBy { get; set; } = "CreatedDate Desc";
    }
}