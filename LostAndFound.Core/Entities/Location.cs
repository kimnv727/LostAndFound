using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Entities.Common;

namespace LostAndFound.Core.Entities
{
    public class Location : ISoftDeleteLiteEntity
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
        
        public virtual Campus Campus { get; set; }  
        public ICollection<Item> Items { get; set; }
        public ICollection<Post> Posts { get;set; }
    }
}