using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Storage
{
    public class StorageUpdateDTO
    {
        //Let them choose from a dropdown of Campus List first
        [Required]
        public int CampusId { get; set; }
        //Then show and let them choose from a dropdown of location List
        [Required]
        public string Location { get; set; }
        //Let them choose from a dropdown of Storage Manager List
        [Required]
        public string MainStorageManagerId { get; set; }
    }
}