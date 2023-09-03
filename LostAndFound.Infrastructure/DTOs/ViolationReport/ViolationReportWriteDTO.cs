using LostAndFound.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ViolationReport
{
    public class ViolationReportWriteDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public ViolationCategory Category { get; set; }
    }
}
