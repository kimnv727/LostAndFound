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
    public class Storage : ICreatedEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        [ForeignKey("Campus")]
        public int CampusId { get; set; }
        public bool IsActive { get; set; }
        public string MainStorageManagerId { get; set; }
        public DateTime CreatedDate { get; set; }

        //Foreign key tables
        public ICollection<Cabinet> Cabinets { get; set; }
        public virtual Campus Campus { get; set; }
    }
}
