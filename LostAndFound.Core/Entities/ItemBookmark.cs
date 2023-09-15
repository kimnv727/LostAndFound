using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Entities.Common;

namespace LostAndFound.Core.Entities
{
    public class ItemBookmark
    {
        [Required]
        [Key, Column(Order = 0)]
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        
        [Required]
        [Key, Column(Order = 1)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        
        public bool IsActive { get; set; }
        
        public virtual Item Item { get; set; }
        public virtual User User { get; set; }
    }
}