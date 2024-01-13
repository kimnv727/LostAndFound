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
        void SendMailToVerifySuccess(string receiverEmail);
        void SendMailToVerifyFail(string receiverEmail);
        void SendMailPostApprove(string receiverEmail, string postName);
        void SendMailItemApprove(string receiverEmail, string itemName);
        void SendMailPostReject(string receiverEmail, string postName);
        void SendMailItemReject(string receiverEmail, string itemName);
        void SendMailReportA(string receiverEmail, string itemName);
        void SendMailReportBSuccess(string receiverEmail, string itemName);
        void SendMailReportBFail(string receiverEmail, string itemName);
        void SendMailReportDenied(string receiverEmail, string itemName);
    }
}