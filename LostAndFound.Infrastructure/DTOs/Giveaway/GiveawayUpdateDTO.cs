using System;

namespace LostAndFound.Infrastructure.DTOs.Giveaway
{
    public class GiveawayUpdateDTO
    {
        public int ItemId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
    }
}