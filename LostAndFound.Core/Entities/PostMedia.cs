using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Entities
{
    public class PostMedia
    {
        [Required]
        [ForeignKey("Post")]
        [Key, Column(Order = 0)]
        public int PostId { get; set; }
        [Required]
        [ForeignKey("Media")]
        [Key, Column(Order = 1)]
        public Guid MediaId { get; set; }

        //public PostMediaType PostMediaType { get; set; }

        public virtual Post Post { get; set; }
        public virtual Media Media { get; set; }
    }
}
