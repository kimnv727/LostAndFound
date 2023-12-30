using LostAndFound.Core.Entities.Common;
using LostAndFound.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public DateTime CreatedDate { get; set; }
        public ReportStatus Status { get; set; }

        public ReportCategory Category { get; set; }

        public virtual ICollection<UserReport> UserReports { get; set; }
    }
}
