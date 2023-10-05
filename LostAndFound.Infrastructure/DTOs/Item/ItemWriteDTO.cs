using System.ComponentModel.DataAnnotations;
using LostAndFound.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemWriteDTO
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string FoundLocation { get; set; }

        public int CategoryId { get; set; }
        
        public int LocationId { get; set; }

        [Required]
        public IFormFile[] Medias { get; set; }
        
    }
}
