using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Entities.Common;

namespace LostAndFound.Core.Entities
{
    public class GiveawayParticipant : ICreatedEntity
    {
        [Required]
        [ForeignKey("Giveaway")]
        [Key, Column(Order = 0)]
        public int GiveawayId { get; set; }
        [Required]
        [ForeignKey("User")]
        [Key, Column(Order = 1)]
        public string UserId { get; set; }
        public bool IsActive { get; set; }
        public bool IsChosenAsWinner { get; set; } = false;
        public bool IsWinner { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        
        //Foreign keys
        public virtual Giveaway Giveaway { get; set; }
        public virtual User User { get; set; }
    }
}