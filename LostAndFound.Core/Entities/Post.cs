using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.Core.Entities.Common;
using LostAndFound.Core.Enums;

namespace LostAndFound.Core.Entities
{
    public class Post : IAuditedEntity, IPostSoftDeleteEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string PostUserId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string PostContent { get; set; }
        [ForeignKey("Location")]
        public int PostLocationId { get; set; }
        [ForeignKey("Category")]
        public int PostCategoryId { get; set; }
        
        public PostStatus PostStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        //Foreign key tables
        public virtual User User { get; set; }
        public virtual Location Location { get; set; }
        public virtual Category Category { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<PostMedia> PostMedias { get; set; }
        public ICollection<PostBookmark> PostBookmarks { get; set; }
        public ICollection<PostFlag> PostFlags { get; set; }
    }
}