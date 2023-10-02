using System;

namespace LostAndFound.Infrastructure.DTOs.Giveaway
{
    public class GiveawayUpdateDTO
    {
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
    }
}