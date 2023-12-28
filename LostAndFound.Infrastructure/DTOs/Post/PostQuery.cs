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
        /*public int PostLocationId { get; set; }
        public int? PostLocationFloor { get; set; }
        public int?[] PostCategoryId { get; set; }
        public int PostCategoryGroupId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }*/
        public int?[] PostCategory { get; set; }
        public int?[] PostLocation { get; set; }
        public string? LostDateFrom { get; set; }
        public string? LostDateTo { get; set; }
        public int CampusId { get; set; }
        /*[DefaultValue(CampusLocationQuery.ALL)]
        public CampusLocationQuery CampusLocation { get; set; }

        public enum CampusLocationQuery
        {
            ALL,
            HO_CHI_MINH,
            DA_NANG,
            HA_NOI,
            CAN_THO
        }*/
        public string SearchText { get; set; }
        public string OrderBy { get; set; } = "CreatedDate Desc";
    }
}