namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool VerifyCorrectPassword(string password, string hashedPassword);
    }
}