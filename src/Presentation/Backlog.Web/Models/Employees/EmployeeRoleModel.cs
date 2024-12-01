using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Employees
{
    public class EmployeeRoleModel : BaseModel
    {
        [LocalizedDisplayName("EmployeeRoleModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("EmployeeRoleModel.SystemName")]
        public string SystemName { get; set; }

        [LocalizedDisplayName("EmployeeRoleModel.Description")]
        public string Description { get; set; }

        [LocalizedDisplayName("EmployeeRoleModel.SystemRole")]
        public bool SystemRole { get; set; }

        [LocalizedDisplayName("EmployeeRoleModel.Active")]
        public bool Active { get; set; }
    }
}
