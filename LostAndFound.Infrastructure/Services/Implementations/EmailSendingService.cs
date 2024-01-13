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

        public void SendMailToRegister(string receiverEmail)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Register Notice";

                string filePath = "MailTemplates//SignUp.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[newusername]", receiverEmail);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailResetPassword(string receiverEmail, string newPass)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Reset Password Notice";

                string filePath = "MailTemplates//ResetPassword.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[newusername]", receiverEmail);
                mailText = mailText.Replace("[newPassword]", newPass);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailWhenUserBan(string receiverEmail)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Ban Notice";

                string filePath = "MailTemplates//BanUser.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[newusername]", receiverEmail);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailWhenPostBan(string receiverEmail, string postName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Ban Post Notice";

                string filePath = "MailTemplates//BanPost.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", receiverEmail);
                mailText = mailText.Replace("[postName]", postName);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailWhenItemBan(string receiverEmail, string itemName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Ban Item Notice";

                string filePath = "MailTemplates//BanItem.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", receiverEmail);
                mailText = mailText.Replace("[itemName]", itemName);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailGiveawayWinner(string receiverEmail, string itemName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Giveaway Winner Notice";

                string filePath = "MailTemplates//GiveawayWinner.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[newusername]", receiverEmail);
                mailText = mailText.Replace("[itemName]", itemName);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailGiveawayReroll(string receiverEmail, string itemName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Giveaway Notice";

                string filePath = "MailTemplates//GiveawayReroll.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[newusername]", receiverEmail);
                mailText = mailText.Replace("[itemName]", itemName);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailToVerifySuccess(string receiverEmail)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Account Verify Notice";

                string filePath = "MailTemplates//VerifySuccess.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[newusername]", receiverEmail);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailToVerifyFail(string receiverEmail)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Account Verify Notice";

                string filePath = "MailTemplates//VerifyFail.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[newusername]", receiverEmail);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailItemApprove(string receiverEmail, string itemName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Approve Item Notice";

                string filePath = "MailTemplates//ApproveItem.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", receiverEmail);
                mailText = mailText.Replace("[itemName]", itemName);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailItemReject(string receiverEmail, string itemName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Reject Item Notice";

                string filePath = "MailTemplates//RejectItem.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", receiverEmail);
                mailText = mailText.Replace("[itemName]", itemName);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailPostApprove(string receiverEmail, string postName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Approve Post Notice";

                string filePath = "MailTemplates//ApprovePost.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", receiverEmail);
                mailText = mailText.Replace("[postName]", postName);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailPostReject(string receiverEmail, string postName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Reject Post Notice";

                string filePath = "MailTemplates//RejectPost.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", receiverEmail);
                mailText = mailText.Replace("[postName]", postName);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailReportA(string receiverEmail, string itemName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Report Notice";

                string filePath = "MailTemplates//ReportA.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", receiverEmail);
                mailText = mailText.Replace("[itemName]", itemName);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailReportBSuccess(string receiverEmail, string itemName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Report Notice";

                string filePath = "MailTemplates//ReportBSuccess.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", receiverEmail);
                mailText = mailText.Replace("[itemName]", itemName);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailReportBFail(string receiverEmail, string itemName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Report Notice";

                string filePath = "MailTemplates//ReportBFail.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", receiverEmail);
                mailText = mailText.Replace("[itemName]", itemName);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public void SendMailReportDenied(string receiverEmail, string itemName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailServiceConfigDto.EmailSender);
                mail.To.Add(receiverEmail);
                mail.Subject = "Report Notice";

                string filePath = "MailTemplates//ReportDenied.html";
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", receiverEmail);
                mailText = mailText.Replace("[itemName]", itemName);

                mail.Body = mailText;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_emailServiceConfigDto.EmailSender,
                        _emailServiceConfigDto.EmailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}