using System;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Giveaway
{
    public class GiveawayQueryExcludeNotStarted : PaginatedQuery, IOrderedQuery
    {
        public int ItemCategoryGroupId { get; set; }
        public int ItemCategoryId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string OrderBy { get; set; } = "StartAt Desc";
    }
}