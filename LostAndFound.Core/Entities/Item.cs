using LostAndFound.Core.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostAndFound.Core.Entities
{
    public class Item : IAuditedEntity, ISoftDeleteEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string Found_User_Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Found_Location { get; set; }
        
        [ForeignKey("Category")]
        public int Category_Id { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public Guid? DeletedBy { get; set; }


    }
}
