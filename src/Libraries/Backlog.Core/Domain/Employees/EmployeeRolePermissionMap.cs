using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Employees
{
    public class EmployeeRolePermissionMap : BaseEntity
    {
        public int PermissionId { get; set; }

        public int EmployeeRoleId { get; set; }

        public virtual EmployeeRole EmployeeRole { get; set; }

        public virtual EmployeeRolePermission Permission { get; set; }
    }
}