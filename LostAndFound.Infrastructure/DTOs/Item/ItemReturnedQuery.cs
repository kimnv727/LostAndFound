using LostAndFound.Infrastructure.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemReturnedQuery : PaginatedQuery, IOrderedQuery
    {

        public int Id { get; set; }

        public string FoundUserId { get; set; }

        public int[] LocationId { get; set; }

        public int? Floor { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int[] CategoryId { get; set; }

        public int[] CategoryGroupId { get; set; }

        public int CabinetId { get; set; }

        public bool IsInStorage { get; set; }

        public string? FoundDateFrom { get; set; }
        public string? FoundDateTo { get; set; }
        public int CampusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ReturnedDateFrom { get; set; }
        public DateTime ReturnedDateTo { get; set; }
        public string OrderBy { get; set; } = "Id DESC";

    }
}
