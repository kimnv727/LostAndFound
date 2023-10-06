using LostAndFound.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Entities
{
    public class UserMedia
    {
        [Required]
        [ForeignKey("User")]
        [Key, Column(Order = 0)]
        public string UserId { get; set; }
        [Required]
        [ForeignKey("Media")]
        [Key, Column(Order = 1)]
        public Guid MediaId { get; set; }
        public UserMediaType? MediaType { get; set; }
        public virtual User User { get; set; }
        public virtual Media Media { get; set; }
    }
}
