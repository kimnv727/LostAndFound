using LostAndFound.Infrastructure.Services.Interfaces;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class PasswordHasherService : IPasswordHasherService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 8);
        }

        public bool VerifyCorrectPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}