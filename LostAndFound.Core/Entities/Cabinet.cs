using LostAndFound.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Entities
{
    public class Cabinet : ICreatedEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [ForeignKey("Storage")]
        public int StorageId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public virtual Storage Storage { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
