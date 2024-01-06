namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IEmailSendingService
    {
        void SendMailToRegister(string receiverEmail);
        void SendMailResetPassword(string receiverEmail, string newPass);
        void SendMailWhenUserBan(string receiverEmail);
        void SendMailWhenPostBan(string receiverEmail, string postName);
        void SendMailWhenItemBan(string receiverEmail, string itemName);
        void SendMailGiveawayWinner(string receiverEmail, string itemName);
        void SendMailGiveawayReroll(string receiverEmail, string itemName);
    }
}