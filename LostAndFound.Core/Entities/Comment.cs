using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Entities.Common;
using LostAndFound.Core.Enums;

namespace LostAndFound.Core.Entities
{
    public class Comment : IAuditedEntity, ISoftDeleteEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string CommentUserId { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
        [Required]
        public string CommentContent { get; set; }
        [Required]
        public string CommentPath{ get; set; }
        
        public bool CommentStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        //Foreign key tables
        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
    }
}