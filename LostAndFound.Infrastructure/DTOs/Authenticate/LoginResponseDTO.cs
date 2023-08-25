namespace LostAndFound.Infrastructure.DTOs.Authenticate
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        
        public string RefreshToken { get; set; }
    }
}