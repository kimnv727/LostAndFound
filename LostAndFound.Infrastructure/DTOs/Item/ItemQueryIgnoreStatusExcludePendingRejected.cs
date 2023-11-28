using LostAndFound.Infrastructure.DTOs.Common;
using System;
using System.ComponentModel;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemQueryIgnoreStatusExcludePendingRejected : PaginatedQuery, IOrderedQuery
    {

        public int Id { get; set; }

        public string FoundUserId { get; set; }

        public int LocationId { get; set; }

        public int? Floor { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int[] CategoryId { get; set; }

        public int CategoryGroupId { get; set; }

        public int CabinetId { get; set; }

        public bool IsInStorage { get; set; }

        //Status = Pending / Active / Returned / Closed / Rejected
        [DefaultValue(ItemStatusQuery.ALL)]
        public ItemStatusQuery ItemStatus { get; set; }

        public enum ItemStatusQuery
        {
            ALL,
            ACTIVE,
            RETURNED,
            CLOSED,
        }

        public DateTime FoundDate { get; set; }

        public DateTime CreatedDate { get; set; }
        public string OrderBy { get; set; } = "Id DESC";

    }
}
