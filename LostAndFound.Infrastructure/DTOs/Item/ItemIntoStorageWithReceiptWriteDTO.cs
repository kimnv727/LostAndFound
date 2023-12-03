using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemIntoStorageWithReceiptWriteDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int CabinetId { get; set; }
        public string? FoundDate { get; set; }
        [Required]
        public IFormFile[] ItemMedias { get; set; }
        [Required]
        public IFormFile ReceiptMedia { get; set; }
    }
}
