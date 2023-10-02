using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Entities.Common;
using LostAndFound.Core.Enums;

namespace LostAndFound.Core.Entities
{
    public class Giveaway : ICreatedEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        [Required]
        public DateTime? StartAt { get; set; }
        [Required]
        public DateTime? EndAt { get; set; }

        public GiveawayStatus GiveawayStatus { get; set; }
        public DateTime CreatedDate { get; set; }

        //Foreign key tables
        public virtual Item Item { get; set; }
        public ICollection<GiveawayParticipant> GiveawayParticipants { get; set; }
    }
}