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
        
        [Required]
        [StringLength(12, MinimumLength = 6)]
        [RegularExpression("^[0-9]{6,12}$", ErrorMessage = "Social Identity Number from 6 to 12 numbers.")]
        public string Sin{ get; set; } //Social Identity Number
        public Gender Gender { get; set; }
        
        [Required]
        [StringLength(70, MinimumLength = 2)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email{ get; set; }
        
        [Required]
        [StringLength(100,ErrorMessage = "Password length must be within 8 - 100 characters",MinimumLength = 8)]
        public string Password{ get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression("(\\+\\d{1,3}\\s?)?((\\(\\d{3}\\)\\s?)|(\\d{3})(\\s|-?))(\\d{3}(\\s|-?))(\\d{4})(\\s?(([E|e]xt[:|.|]?)|x|X)(\\s?\\d+))?", ErrorMessage = "Phone number must be of right format")] //10-digit phone number
        public string PhoneNumber{ get; set; }

        public string Avatar{ get; set; }
    }
}