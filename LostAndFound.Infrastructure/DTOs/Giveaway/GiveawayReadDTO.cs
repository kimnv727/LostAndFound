using System;
using System.Collections.Generic;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.GiveawayParticipant;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.User;

namespace LostAndFound.Infrastructure.DTOs.Giveaway
{
    public class GiveawayReadDTO
    {
        public int Id { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public GiveawayStatus GiveawayStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ParticipantsCount { get; set; }
        public UserReadDTO? WinnerUser { get; set; }
        public ItemReadDTO Item { get; set; }
        public ICollection<GiveawayParticipantReadDTO> GiveawayParticipants { get; set; }
    }
}