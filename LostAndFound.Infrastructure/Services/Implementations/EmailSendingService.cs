using System.IO;
using System.Net.Mail;
using LostAndFound.Infrastructure.DTOs.Email;
using LostAndFound.Infrastructure.Services.Interfaces;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class EmailSendingService : IEmailSendingService
    {
        private readonly EmailServiceConfigDTO _emailServiceConfigDto;

        public EmailSendingService(EmailServiceConfigDTO emailServiceConfigDTO = null)
        {
            _emailServiceConfigDto = emailServiceConfigDTO;
        }

        public void SendMailToRegister(string receiverEmail, string password)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Register";

                string filePath = "MailTemplates//SignUp.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[newusername]", receiverEmail);
                mailText = mailText.Replace("[password]", password);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailToRequestPasswordReset(string receiverEmail, string password)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Request Password Reset Completed";

                string fileName = "MailTemplates//ResetPassword.html";
                StreamReader str = new StreamReader(fileName);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[newusername]", receiverEmail);
                mailText = mailText.Replace("[password]", password);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailInformSuccessPasswordChange(string receiverEmail)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Password Has Been Successfully Changed";

                string filePath = "MailTemplates//ChangePasswordSuccess.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[newusername]", receiverEmail);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}