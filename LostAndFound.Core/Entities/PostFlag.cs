using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Enums;

namespace LostAndFound.Core.Entities
{
    public class PostFlag
    {
        [Required]
        [ForeignKey("Post")]
        [Key, Column(Order = 0)]
        public int PostId { get; set; }
        [Required]
        [ForeignKey("User")]
        [Key, Column(Order = 1)]
        public string UserId { get; set; }
        public PostFlagReason PostFlagReason { get; set; }
        public bool IsActive { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}