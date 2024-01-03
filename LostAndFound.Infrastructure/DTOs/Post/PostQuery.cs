using System;
using System.ComponentModel;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostQuery : PaginatedQuery, IOrderedQuery 
    {
        public string PostUserId { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public int?[] PostCategory { get; set; }
        public int?[] PostLocation { get; set; }
        public DateTime? LostDateFrom { get; set; }
        public DateTime? LostDateTo { get; set; }
        public int CampusId { get; set; }
        public string SearchText { get; set; }
        public string OrderBy { get; set; } = "CreatedDate Desc";
    }
}