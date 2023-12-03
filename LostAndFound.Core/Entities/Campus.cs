using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Entities.Common;
using LostAndFound.Core.Enums;

namespace LostAndFound.Core.Entities
{
    public class Campus
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Address { get; set; }

        [Required]
        public CampusLocation CampusLocation { get; set; }
        
        [Required]
        public bool? IsActive { get; set; }
        
        public ICollection<Location> Locations { get; set; }
    }
}