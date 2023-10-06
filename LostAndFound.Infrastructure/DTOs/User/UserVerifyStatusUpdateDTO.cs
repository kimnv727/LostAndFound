using LostAndFound.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Infrastructure.DTOs.User
{
    public class UserVerifyStatusUpdateDTO
    {
        [Required]
        public string UserId{ get; set; }
        [Required]
        public UserVerifyStatus VerifyStatus { get; set; }
    }
}