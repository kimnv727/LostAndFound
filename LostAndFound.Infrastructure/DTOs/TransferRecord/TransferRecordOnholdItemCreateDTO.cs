using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Receipt
{
    public class TransferRecordOnholdItemCreateDTO
    {
        [Required]
        public string SenderId { get; set; }

        [Required]
        public int ReportId { get; set; }

        [Required]
        public int CabinetId { get; set; }
    }
}
