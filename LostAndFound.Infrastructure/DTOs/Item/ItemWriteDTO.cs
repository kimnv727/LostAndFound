using System.ComponentModel.DataAnnotations;
using LostAndFound.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemWriteDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int LocationId { get; set; }

        public int? CabinetId { get; set; }

        //public ItemStatus ItemStatus { get; set; }
        //On Front will be a dropdown 
        public string? FoundDate { get; set; }

        [Required]
        public IFormFile[] Medias { get; set; }
        
    }
}
