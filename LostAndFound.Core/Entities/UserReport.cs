using LostAndFound.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostAndFound.Core.Entities
{
    public class UserReport
    {
        [Required]
        [ForeignKey("User")]
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        [Required]
        [ForeignKey("Report")]
        [Key, Column(Order = 1)]
        public int ReportId { get; set; }

        public ReportType Type { get; set; }

        public virtual User User { get; set; }

        public virtual Report Report { get; set; }
    }
}
