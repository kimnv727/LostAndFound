using LostAndFound.Infrastructure.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ItemClaim
{
    public class ItemClaimWithUserReadDTO
    {
        public int ItemId { get; set; }
        public string UserId { get; set; }
        public bool ClaimStatus { get; set; }
        public DateTime ClaimDate { get; set; }

        public UserReadDTO User { get; set; }
    }
}
