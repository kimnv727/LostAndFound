using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ItemClaim
{
    public class ItemClaimReadDTO
    {
        public int ItemId { get; set; }
        public string UserId { get; set; }
        public ClaimStatus ClaimStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime ClaimDate { get; set; }

        /*public ItemReadDTO Item { get; set; } 
        public UserReadDTO User { get; set; }*/
    }
}
