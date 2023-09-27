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
        
        //TODO: wait Kim change Category and Item crud for this
        /*[Required]
        public ItemValue Value { get; set; }*/

    }
}
