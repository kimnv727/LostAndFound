using System;
using System.ComponentModel;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Giveaway
{
    public class GiveawayQueryExcludeNotStarted : PaginatedQuery, IOrderedQuery
    {
        public int ItemCategoryGroupId { get; set; }
        public int ItemCategoryId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public enum GiveawayStatusQuery
        {
            All,
            ONGOING,
            REWARD_DISTRIBUTION_IN_PROGRESS,
            CLOSED
        }
        [DefaultValue(GiveawayStatusQuery.All)]
        public GiveawayStatusQuery GiveawayStatus { get; set; }
        public int CampusId { get; set; }
        public string OrderBy { get; set; } = "StartAt Desc";
    }
}