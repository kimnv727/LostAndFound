using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostAndFound.Core.Entities
{
    public class UserDevice
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? Token { get; set; }

        [ForeignKey("User")]
        [Key, Column(Order = 1)]
        public string? UserId { get; set; }

        public virtual User User { get; set; }
    }
}