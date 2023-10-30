using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Entities
{
    public class Storage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public int CampusId { get; set; }
        public bool IsActive { get; set; }
        public int MainStorageManagerId { get; set; }
        public DateTime CreatedDate { get; set; }

        //Foreign key tables
        public ICollection<Cabinet> Cabinets { get; set; }
    }
}
