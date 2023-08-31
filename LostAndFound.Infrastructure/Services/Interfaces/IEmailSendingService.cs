namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IEmailSendingService
    {
        void SendMailToRegister(string receiverEmail, string password);
        void SendMailInformSuccessPasswordChange(string receiverEmail);
        void SendMailToRequestPasswordReset(string receiverEmail, string password);
    }
}