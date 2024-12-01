using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Messages
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailAccount emailAccount, string subject, string body,
            string fromAddress, string fromName, string toAddress, string toName);
    }
}
