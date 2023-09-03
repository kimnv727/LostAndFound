using LostAndFound.Core.Entities.Common;
using LostAndFound.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Core.Entities
{
    public class ViolationReport : ICreatedEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public ViolationStatus Status { get; set; }

        public ViolationCategory Category { get; set; }

        public virtual ICollection<UserViolationReport> UserViolationReports { get; set; }
    }
}
