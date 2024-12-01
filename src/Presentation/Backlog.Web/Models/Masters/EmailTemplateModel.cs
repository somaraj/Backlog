using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Models.Masters
{
    public class EmailTemplateModel : BaseModel
    {
        public EmailTemplateModel()
        {
            AvailableEmailAccounts = [new SelectListItem { Text = "Select", Value = "" }];
        }

        [LocalizedDisplayName("EmailAccountModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("EmailAccountModel.EmailSubject")]
        public string EmailSubject { get; set; }

        [LocalizedDisplayName("EmailAccountModel.EmailBody")]
        public string EmailBody { get; set; }

        [LocalizedDisplayName("EmailAccountModel.EmailAccount")]
        public int EmailAccountId { get; set; }

        [LocalizedDisplayName("EmailAccountModel.Active")]
        public bool Active { get; set; }

        public IList<SelectListItem> AvailableEmailAccounts { get; set; }
    }

    public class EmailTemplateGridModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string EmailSubject { get; set; }

        public string EmailAccount { get; set; }

        public bool Active { get; set; }
    }
}