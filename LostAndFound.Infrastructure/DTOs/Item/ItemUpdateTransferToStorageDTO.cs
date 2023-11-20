using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemUpdateTransferToStorageDTO
    {
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int CabinetId { get; set; }
        [Required]
        public IFormFile ReceiptMedia { get; set; }
    }
}
