using System;
using System.ComponentModel;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Giveaway
{
    public class GiveawayQueryWithStatus : PaginatedQuery, IOrderedQuery 
    {
        public int ItemCategoryGroupId { get; set; }
        public int ItemCategoryId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public enum GiveawayStatusQuery
        {
            All,
            NOT_STARTED,
            ONGOING,
            WAITING_RESULT,
            CLOSED
        }
        [DefaultValue(GiveawayStatusQuery.All)]
        public GiveawayStatusQuery GiveawayStatus { get; set; }
        public string OrderBy { get; set; } = "StartAt Desc";
    }
}