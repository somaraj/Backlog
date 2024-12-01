using System.ComponentModel.DataAnnotations;
using Backlog.Core.Common;
using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Employees
{
    public class EmployeeModel : BaseModel
    {
        public EmployeeModel()
        {
            SelectedRoleIds = [];
            Status = (int)EmployeeStatusEnum.Active;
            EmailWelcomeKit = true;
        }

        [LocalizedDisplayName("EmployeeModel.FirstName")]
        public string FirstName { get; set; }

        [LocalizedDisplayName("EmployeeModel.LastName")]
        public string LastName { get; set; }

        [LocalizedDisplayName("EmployeeModel.MobileNumber")]
        public string MobileNumber { get; set; }

        [LocalizedDisplayName("EmployeeModel.Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [LocalizedDisplayName("EmployeeModel.Gender")]
        public int GenderId { get; set; }

        [LocalizedDisplayName("EmployeeModel.DateOfBirth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateOnly? DateOfBirth { get; set; }

        [LocalizedDisplayName("EmployeeModel.DateOfJoin")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateOnly? DateOfJoin { get; set; }

        [LocalizedDisplayName("EmployeeModel.Department")]
        public int DepartmentId { get; set; }

        [LocalizedDisplayName("EmployeeModel.Designation")]
        public int DesignationId { get; set; }

        [LocalizedDisplayName("EmployeeModel.Language")]
        public int LanguageId { get; set; }

        [LocalizedDisplayName("EmployeeModel.Role")]
        public List<int> SelectedRoleIds { get; set; }

        [LocalizedDisplayName("EmployeeModel.Status")]
        public int Status { get; set; }

        public string Name { get; set; }

        [LocalizedDisplayName("EmployeeModel.EmailWelcomeKit")]
        public bool EmailWelcomeKit { get; set; }
    }
}
