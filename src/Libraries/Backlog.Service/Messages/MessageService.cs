using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Service.Masters;
using Backlog.Service.Security;
using System.Net;

namespace Backlog.Service.Messages
{
    public class MessageService : IMessageService
    {
        #region Fields

        private readonly IEmailAccountService _emailAccounteService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailService _emailSenderService;
        private readonly IEncryptionService _encryptionService;
        private readonly IHttpHelper _httpHelper;

        #endregion

        #region Ctor

        public MessageService(IEmailAccountService emailAccountService,
            IEmailTemplateService emailTemplateService,
            IEmailService emailSenderService,
            IHttpHelper httpHelper,
            IEncryptionService encryptionService)
        {
            _emailAccounteService = emailAccountService;
            _emailTemplateService = emailTemplateService;
            _emailSenderService = emailSenderService;
            _httpHelper = httpHelper;
            _encryptionService = encryptionService;
        }

        #endregion

        #region Account

        public async Task EmailPasswordResetLinkAsync(Employee employee, string token)
        {
            var emailTemplate = await _emailTemplateService.GetByNameAsync(EmailTemplateTypeEnum.ResetPassword.ToString());
            if (emailTemplate != null && !string.IsNullOrEmpty(emailTemplate.EmailBody))
            {
                var emailAccount = await _emailAccounteService.GetByIdAsync(emailTemplate.EmailAccountId);
                if (emailAccount.Active)
                {
                    var url = $"{_httpHelper.GetBaseURL()}/setpassword?token={WebUtility.UrlEncode(token)}";
                    var body = emailTemplate.EmailBody;
                    body = body.Replace("%displayname%", $"{employee.FirstName} {employee.LastName}");
                    body = body.Replace("%url%", url);
                    body = body.Replace("%year%", DateTime.Today.Year.ToString());
                    body = body.Replace("%company%", Constant.CompanyName);

                    await _emailSenderService.SendEmailAsync(emailAccount, emailTemplate.EmailSubject, body,
                        emailAccount.FromEmail, emailAccount.FromName, employee.Email, $"{employee.FirstName} {employee.LastName}");
                }
            }
        }

        public async Task EmailActivationKitAsync(Employee employee)
        {
            var emailTemplate = await _emailTemplateService.GetByNameAsync(EmailTemplateTypeEnum.Activation.ToString());
            if (emailTemplate != null && !string.IsNullOrEmpty(emailTemplate.EmailBody))
            {
                var emailAccount = await _emailAccounteService.GetByIdAsync(emailTemplate.EmailAccountId);
                if (emailAccount.Active)
                {
                    var token = _encryptionService.GenerateToken(employee.Code);
                    var url = $"{_httpHelper.GetBaseURL()}/activate?token={token}";
                    var body = emailTemplate.EmailBody;
                    body = body.Replace("%display_name%", $"{employee.FirstName} {employee.LastName}");
                    body = body.Replace("%body%", $"Please click the link below to activate your account.");
                    body = body.Replace("%url%", url);
                    body = body.Replace("%year%", DateTime.Today.Year.ToString());
                    body = body.Replace("%company%", Constant.CompanyName);

                    await _emailSenderService.SendEmailAsync(emailAccount, emailTemplate.EmailSubject, body,
                        emailAccount.FromEmail, emailAccount.FromName, employee.Email, $"{employee.FirstName} {employee.LastName}");
                }
            }
        }

        public async Task EmailWelcomeKitAsync(Employee employee)
        {
            var emailTemplate = await _emailTemplateService.GetByNameAsync(EmailTemplateTypeEnum.WelcomeKit.ToString());
            if (emailTemplate != null && !string.IsNullOrEmpty(emailTemplate.EmailBody))
            {
                var emailAccount = await _emailAccounteService.GetByIdAsync(emailTemplate.EmailAccountId);
                if (emailAccount.Active)
                {
                    var url = $"{_httpHelper.GetBaseURL()}";
                    var body = emailTemplate.EmailBody;
                    body = body.Replace("%display_name%", $"{employee.FirstName} {employee.LastName}");
                    body = body.Replace("%button%", url);
                    body = body.Replace("%year%", DateTime.Today.Year.ToString());
                    body = body.Replace("%company%", Constant.CompanyName);

                    await _emailSenderService.SendEmailAsync(emailAccount, emailTemplate.EmailSubject, body,
                         emailAccount.FromEmail, emailAccount.FromName, employee.Email, $"{employee.FirstName} {employee.LastName}");
                }
            }
        }

        public async Task EmailRegistrationKitAsync(Employee employee)
        {
            var emailTemplate = await _emailTemplateService.GetByNameAsync(EmailTemplateTypeEnum.RegistrationKit.ToString());
            if (emailTemplate != null && !string.IsNullOrEmpty(emailTemplate.EmailBody))
            {
                var emailAccount = await _emailAccounteService.GetByIdAsync(emailTemplate.EmailAccountId);
                if (emailAccount.Active)
                {
                    var url = $"{_httpHelper.GetBaseURL()}";
                    var body = emailTemplate.EmailBody;
                    body = body.Replace("%display_name%", $"{employee.FirstName} {employee.LastName}");
                    body = body.Replace("%button%", url);
                    body = body.Replace("%year%", DateTime.Today.Year.ToString());
                    body = body.Replace("%company%", Constant.CompanyName);

                    await _emailSenderService.SendEmailAsync(emailAccount, emailTemplate.EmailSubject, body,
                        emailAccount.FromEmail, emailAccount.FromName, employee.Email, $"{employee.FirstName} {employee.LastName}");
                }
            }
        }

        public async Task EmailNotificationAsync(Employee employee, string content)
        {
            var emailTemplate = await _emailTemplateService.GetByNameAsync(EmailTemplateTypeEnum.Notification.ToString());
            if (emailTemplate != null && !string.IsNullOrEmpty(emailTemplate.EmailBody))
            {
                var emailAccount = await _emailAccounteService.GetByIdAsync(emailTemplate.EmailAccountId);
                if (emailAccount.Active)
                {
                    var url = $"{_httpHelper.GetBaseURL()}";
                    var body = emailTemplate.EmailBody;
                    body = body.Replace("%display_name%", $"{employee.FirstName} {employee.LastName}");
                    body = body.Replace("%button%", url);
                    body = body.Replace("%year%", DateTime.Today.Year.ToString());
                    body = body.Replace("%company%", Constant.CompanyName);
                    body = body.Replace("%body%", content);

                    await _emailSenderService.SendEmailAsync(emailAccount, emailTemplate.EmailSubject, body,
                        emailAccount.FromEmail, emailAccount.FromName, employee.Email, $"{employee.FirstName} {employee.LastName}");
                }
            }
        }

        #endregion
    }
}