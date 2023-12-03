﻿using LostAndFound.Infrastructure.DTOs.Common;
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

        public string? FoundDateFrom { get; set; }
        public string? FoundDateTo { get; set; }

        [DefaultValue(CampusLocationQuery.ALL)]
        public CampusLocationQuery CampusLocation { get; set; }

        public enum CampusLocationQuery
        {
            ALL,
            HO_CHI_MINH,
            DA_NANG,
            HA_NOI,
            CAN_THO
        }
        public DateTime CreatedDate { get; set; }
        public string OrderBy { get; set; } = "Id DESC";

    }
}
