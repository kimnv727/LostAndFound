namespace LostAndFound.Infrastructure.DTOs.User
{
    public class UserUpdatePasswordDTO
    {
        public string oldPassword{ get; set; }
        
        public string NewPassword { get; set; }
    }
}