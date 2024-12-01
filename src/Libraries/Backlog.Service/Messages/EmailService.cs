using Backlog.Core.Domain.Masters;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net;

namespace Backlog.Service.Messages
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(EmailAccount emailAccount,
            string subject,
            string body,
            string fromAddress,
            string fromName,
            string toAddress,
            string toName)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(fromName, fromAddress));
            message.To.Add(new MailboxAddress(toName, toAddress));

            message.Subject = subject;
            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = builder.ToMessageBody();
            var smtpClient = new SmtpClient();
            try
            {
                //Configure client
                smtpClient.Connect(emailAccount.Host, emailAccount.Port, emailAccount.EnableSsl);
                if (emailAccount.UseDefaultCredentials)
                {
                    smtpClient.Authenticate(CredentialCache.DefaultNetworkCredentials);
                }
                else if (!string.IsNullOrWhiteSpace(emailAccount.UserName))
                {
                    smtpClient.Authenticate(new NetworkCredential(emailAccount.UserName, emailAccount.Password));
                }
                //Send email
                var response = await smtpClient.SendAsync(message);
                smtpClient.Disconnect(true);
            }
            catch (Exception ex)
            {
                smtpClient.Dispose();
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
