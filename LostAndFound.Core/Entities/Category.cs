using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LostAndFound.Core.Enums;
using System.Collections.Generic;
using LostAndFound.Core.Entities.Common;

namespace LostAndFound.Core.Entities
{
    public class Category : ISoftDeleteLiteEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("CategoryGroup")]
        public int CategoryGroupId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsSensitive { get; set; }
        [Required]
        public ItemValue Value { get; set; }
        public virtual CategoryGroup CategoryGroup { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
