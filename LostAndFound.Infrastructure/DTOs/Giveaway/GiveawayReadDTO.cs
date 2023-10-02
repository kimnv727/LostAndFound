using System;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Item;

namespace LostAndFound.Infrastructure.DTOs.Giveaway
{
    public class GiveawayReadDTO
    {
        public int Id { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public GiveawayStatus GiveawayStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public ItemReadDTO Item { get; set; }
    }
}