using LostAndFound.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Entities
{
    public class ItemClaim 
    {
        [Required]
        [ForeignKey("Item")]
        [Key, Column(Order = 0)]
        public int ItemId { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ClaimStatus ClaimStatus { get; set; }
        public DateTime ClaimDate { get; set; }
        public bool IsActive { get; set; }
        public virtual Item Item { get; set; }
        public virtual User User { get; set; }
    }
}
