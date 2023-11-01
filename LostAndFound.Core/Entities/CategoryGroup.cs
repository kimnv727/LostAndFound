using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LostAndFound.Core.Enums;

namespace LostAndFound.Core.Entities
{
    public class CategoryGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name {  get; set; }

        [Required]
        public string Description { get; set; }
        public bool IsSensitive { get; set; }
        [Required]
        public ItemValue Value { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
