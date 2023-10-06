using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.User
{
    public class UserVerifyStatusUpdateDTO
    {
        public string UserId{ get; set; }
        
        public UserVerifyStatus VerifyStatus { get; set; }
    }
}