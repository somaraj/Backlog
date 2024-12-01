using System.Collections.Generic;

namespace Backlog.Web.Models.Employees
{
    public class EmployeeRolePermissionGridModel
    {
        public EmployeeRolePermissionGridModel()
        {
            EmployeeRolePermission = new List<EmployeeRolePermissionModel>();
            EmployeeRolePermissionMap = new List<EmployeeRolePermissionMapModel>();
        }

        public int RoleId { get; set; }

        public bool SystemRole { get; set; }

        public bool IsAdmin { get; set; }

        public IList<EmployeeRolePermissionModel> EmployeeRolePermission { get; set; }

        public IList<EmployeeRolePermissionMapModel> EmployeeRolePermissionMap { get; set; }
    }
}