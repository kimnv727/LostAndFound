using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LostAndFound.Core.Entities.Common;
using LostAndFound.Core.Enums;

namespace LostAndFound.Core.Entities
{
    public class CategoryGroup : ISoftDeleteLiteEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name {  get; set; }

        [Required]
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
