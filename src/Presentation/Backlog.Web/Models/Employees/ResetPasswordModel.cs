using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Backlog.Web.Models.Employees
{
    public class ResetPasswordModel : BaseModel
    {
        [LocalizedDisplayName("ResetPasswordModel.Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
