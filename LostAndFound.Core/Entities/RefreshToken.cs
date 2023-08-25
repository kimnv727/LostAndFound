using System;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Core.Entities
{
    public class RefreshToken
    {
        [Key]
        public Guid ID { get; set; }
        
        [Required]
        public string Value { get; set; }
        
        [Required]
        public Guid TokenID { get; set; }
        
        [Required]
        public DateTime ExpiredAt { get; set; }

        public virtual Token Token { get; set; }
    }
}