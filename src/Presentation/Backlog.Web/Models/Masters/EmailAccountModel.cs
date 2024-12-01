using System.ComponentModel.DataAnnotations;
using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Masters
{
    public class EmailAccountModel : BaseModel
    {
        public EmailAccountModel()
        {
            EmailTemplates = [];
        }

        [LocalizedDisplayName("EmailAccountModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("EmailAccountModel.Description")]
        public string? Description { get; set; }

        [LocalizedDisplayName("EmailAccountModel.UserName")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayName("EmailAccountModel.Password")]
        public string Password { get; set; }

        [LocalizedDisplayName("EmailAccountModel.Host")]
        public string Host { get; set; }

        [LocalizedDisplayName("EmailAccountModel.Port")]
        public int Port { get; set; }

        [LocalizedDisplayName("EmailAccountModel.EnableSsl")]
        public bool EnableSsl { get; set; }

        [DataType(DataType.EmailAddress)]
        [LocalizedDisplayName("EmailAccountModel.FromEmail")]
        public string FromEmail { get; set; }

        [LocalizedDisplayName("EmailAccountModel.FromName")]
        public string FromName { get; set; }

        [LocalizedDisplayName("EmailAccountModel.UseDefaultCredentials")]
        public bool UseDefaultCredentials { get; set; }

        [LocalizedDisplayName("EmailAccountModel.Active")]
        public bool Active { get; set; }

        public IList<EmailTemplateModel> EmailTemplates { get; set; }
    }
}