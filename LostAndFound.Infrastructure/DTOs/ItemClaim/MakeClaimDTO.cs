using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ItemClaim
{
    public class MakeClaimDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public int ItemId { get; set; }
    }
}
