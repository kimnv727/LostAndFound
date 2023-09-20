using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostAndFound.Core.Entities
{
    public class UserDevice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? Token { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual User User { get; set; }
    }
}