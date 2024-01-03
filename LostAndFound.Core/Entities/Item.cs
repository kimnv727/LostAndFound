using LostAndFound.Core.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Enums;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LostAndFound.Core.Entities
{
    public class Item : ICreatedEntity
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

        [ForeignKey("Cabinet")]
        public int? CabinetId { get; set; }
        
        public bool IsInStorage { get; set; }

        //Status = Pending / Active / Returned / Closed
        public ItemStatus ItemStatus { get; set; }

        public string? FoundDate { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public virtual User User { get; set; }
        public virtual Location Location { get; set; }
        public virtual Category Category { get; set; }
        public virtual Cabinet Cabinet {  get; set; }
        public ICollection<ItemMedia> ItemMedias { get; set; }
        public ICollection<ItemClaim> ItemClaims { get; set; }
        public ICollection<ItemFlag> ItemFlags { get; set; }
        public ICollection<TransferRecord> Receipts { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
