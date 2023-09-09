using System.ComponentModel.DataAnnotations;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.User
{
    public class UserUpdateDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName{ get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName{ get; set; }

        public Gender Gender { get; set; }
        
        [Required]
        [StringLength(10)]
        [RegularExpression("(\\+\\d{1,3}\\s?)?((\\(\\d{3}\\)\\s?)|(\\d{3})(\\s|-?))(\\d{3}(\\s|-?))(\\d{4})(\\s?(([E|e]xt[:|.|]?)|x|X)(\\s?\\d+))?", ErrorMessage = "Phone number must be of right format")] //10-digit phone number
        public string Phone{ get; set; }

        public string Avatar{ get; set; }
    }
}