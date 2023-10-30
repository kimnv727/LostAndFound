using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Entities.Common;

namespace LostAndFound.Core.Entities
{
    public class Location 
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        
        [Required]
        public string LocationName { get; set; }
        
        [Required]
        public int Floor { get; set; }
        
        [Required]
        public bool? IsActive { get; set; }
        
        public virtual Campus Property { get; set; }  
    }
}