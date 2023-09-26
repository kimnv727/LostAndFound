using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Core.Entities
{
    public class Category 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("CategoryGroup")]
        public int CategoryGroupId { get; set; }
        //TODO: add field to show low or high value
    }
}
