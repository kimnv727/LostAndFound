using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Entities
{
    public class ReportMedia
    {
        [Required]
        [ForeignKey("Report")]
        [Key, Column(Order = 0)]
        public int ReportId { get; set; }
        [Required]
        [ForeignKey("Media")]
        [Key, Column(Order = 1)]
        public Guid MediaId { get; set; }
        public virtual Report Report { get; set; }
        public virtual Media Media { get; set; }
    }
}
