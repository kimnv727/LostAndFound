using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Infrastructure.DTOs.UserMedia
{
    public class UserMediaCredentialsWriteDTO
    {
        [Required]
        public string SchoolId { get; set; }
        [Required]
        public IFormFile CCID { get; set; }
        [Required]
        public IFormFile StudentCard { get; set; }
    }
}
