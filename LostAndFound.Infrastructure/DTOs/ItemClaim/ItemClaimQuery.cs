using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ItemClaim
{
    public class ItemClaimQuery : PaginatedQuery, IOrderedQuery
    {
        public int ItemId { get; set; }
        public string UserId { get; set; }
        public bool ClaimStatus { get; set; }
        public DateTime ClaimDate { get; set; }
        public string OrderBy { get; set; } = "ClaimDate DESC";
    }
}
