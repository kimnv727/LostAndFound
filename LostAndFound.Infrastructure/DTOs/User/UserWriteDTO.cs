using System.ComponentModel.DataAnnotations;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.User
{
    public class UserWriteDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName{ get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName{ get; set; }
        
        public Gender Gender { get; set; }
        
        [Required]
        [StringLength(70, MinimumLength = 2)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email{ get; set; }
        
        [Required]
        [StringLength(100,ErrorMessage = "Password length must be within 1 - 100 characters",MinimumLength = 1)]
        public string Password{ get; set; }

        public string Avatar{ get; set; }
    }
}