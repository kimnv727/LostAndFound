using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Entities.Common;

namespace LostAndFound.Core.Entities
{
    public class Property : IAuditedEntity, ISoftDeleteEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string PropertyName { get; set; }
        
        [Required]
        public string Address { get; set; }
        
        [Required]
        public bool? IsActive { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}