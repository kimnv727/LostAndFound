using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Cabinet
{
    public class CabinetWriteDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        //Let them choose from a dropdown of Storage List
        public int StorageId { get; set; }
    }
}
