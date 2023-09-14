using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Enums;

namespace LostAndFound.Core.Entities
{
    public class CommentFlag
    {
        [Required]
        [ForeignKey("Comment")]
        [Key, Column(Order = 0)]
        public int CommentId { get; set; }
        [Required]
        [ForeignKey("User")]
        [Key, Column(Order = 1)]
        public string UserId { get; set; }
        public CommentFlagReason CommentFlagReason { get; set; }
        public bool IsActive { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual User User { get; set; }
    }
}