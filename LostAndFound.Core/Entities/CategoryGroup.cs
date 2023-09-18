using System.ComponentModel.DataAnnotations;

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

    }
}
