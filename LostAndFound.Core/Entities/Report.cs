using LostAndFound.Core.Entities.Common;
using LostAndFound.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostAndFound.Core.Entities
{
    public class Report : ICreatedEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public string? ReportComment { get; set; }
        public DateTime CreatedDate { get; set; }
        public ReportStatus Status { get; set; }
        public virtual User User { get; set; }
        public virtual Item Item { get; set; }
        public ICollection<ReportMedia> ReportMedias { get; set; }
    }
}
