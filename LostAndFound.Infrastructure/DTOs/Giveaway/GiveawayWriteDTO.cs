using System;

namespace LostAndFound.Infrastructure.DTOs.Giveaway
{
    public class GiveawayWriteDTO
    {
        public int ItemId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
    }
}