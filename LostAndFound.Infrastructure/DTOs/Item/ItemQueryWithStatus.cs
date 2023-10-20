using LostAndFound.Infrastructure.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;
using static LostAndFound.Infrastructure.DTOs.Post.PostQueryWithStatus;
using System.ComponentModel;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemQueryWithStatus : PaginatedQuery
    {

        public int Id { get; set; }

        public string FoundUserId { get; set; }

        public int LocationId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        //Status = Pending / Active / Returned / Closed / Rejected
        [DefaultValue(ItemStatusQuery.ALL)]
        public ItemStatusQuery ItemStatus { get; set; }

        public enum ItemStatusQuery
        {
            ALL,
            PENDING,
            ACTIVE,
            RETURNED,
            CLOSED,
            REJECTED
        }

        public DateTime FoundDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
