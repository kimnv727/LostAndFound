namespace LostAndFound.Infrastructure.DTOs.Authenticate
{
    public class AuthenticateDTO
    {
        public string Uid { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }
        public string DeviceToken { get; set; }
    }
}