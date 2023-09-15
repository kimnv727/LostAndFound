using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemReadDTO
    {

        public string FoundUserId { get; set; }
        
        public int LocationId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int CategoryId { get; set; }

        //Status = Found / Active / Returned / Closed
        public ItemStatus ItemStatus { get; set; }

        public DateTime FoundDate { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public Guid? DeletedBy { get; set; }

    }
}
