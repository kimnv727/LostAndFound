using LostAndFound.Infrastructure.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemQuery : PaginatedQuery
    {

        public string FoundUserId { get; set; }
        
        public int LocationId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int CategoryId { get; set; }

        //Status = Pending / Active / Returned / Closed / Rejected
        public ItemStatus ItemStatus { get; set; }

        public DateTime FoundDate { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}
