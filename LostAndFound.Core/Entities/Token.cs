using System;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Core.Entities
{
    public class Token
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string Value { get; set; }
        
        public Guid UserID { get; set; }

        public virtual RefreshToken RefreshToken { get; set; }
        
        public virtual User User { get; set; }
    }
}