using LostAndFound.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostAndFound.Core.Entities
{
    public class UserViolationReport
    {
        [Required]
        [ForeignKey("User")]
        [Key, Column(Order = 0)]
        public Guid UserId { get; set; }

        [Required]
        [ForeignKey("Report")]
        [Key, Column(Order = 1)]
        public int ReportId { get; set; }

        public ViolationType Type { get; set; }

        public virtual User User { get; set; }

        public virtual ViolationReport Report { get; set; }
    }
}
