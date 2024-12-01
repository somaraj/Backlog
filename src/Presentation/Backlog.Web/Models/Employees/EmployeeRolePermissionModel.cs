using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Employees
{
    public class EmployeeRolePermissionModel : BaseModel
    {
        [LocalizedDisplayName("EmployeeRolePermissionModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("EmployeeRolePermissionModel.SystemName")]
        public string SystemName { get; set; }

        [LocalizedDisplayName("EmployeeRolePermissionModel.RoleGroup")]
        public string RoleGroup { get; set; }

        [LocalizedDisplayName("EmployeeRolePermissionModel.SystemPermission")]
        public bool SystemPermission { get; set; }
    }
}