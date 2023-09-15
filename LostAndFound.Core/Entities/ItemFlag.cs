using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Enums;

namespace LostAndFound.Core.Entities
{
    public class ItemFlag
    {
        [Required]
        [ForeignKey("Item")]
        [Key, Column(Order = 0)]
        public int ItemId { get; set; }
        
        [Required]
        [ForeignKey("User")]
        [Key, Column(Order = 1)]
        public string UserId { get; set; }
        
        [Required]
        public ItemFlagReason ItemFlagReason { get; set; }
        
        public bool IsActive { get; set; }
        
        public virtual Item Item { get; set; }
        public virtual User User { get; set; }
    }
}