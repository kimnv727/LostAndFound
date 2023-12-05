using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Infrastructure.DTOs.UserMedia
{
    public class UserMediaCredentialsWriteDTO
    {
        [Required]
        public string SchoolId { get; set; }
        [Required]
        public IFormFile CCIDFront { get; set; }
        [Required]
        public IFormFile CCIDBack { get; set; }
        [Required]
        public IFormFile StudentCard { get; set; }
    }
}
