using LostAndFound.Core.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Enums;
using System.Collections.Generic;

namespace LostAndFound.Core.Entities
{
    public class Item : IAuditedEntity, ISoftDeleteEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string FoundUserId { get; set; }

        [Required]
        public int LocationId { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
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

        //Foreign key tables
        public virtual User User { get; set; }
        public virtual Location Location { get; set; }
        public ICollection<ItemMedia> PostMedias { get; set; }


    }
}
