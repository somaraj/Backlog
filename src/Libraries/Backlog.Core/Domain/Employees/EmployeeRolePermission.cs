using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Employees
{
    public class EmployeeRolePermission : BaseEntity
    {
        private ICollection<EmployeeRolePermissionMap> _employeeRolePermissionMaps;

        public string Name { get; set; }

        public string SystemName { get; set; }

        public string RoleGroup { get; set; }

        public bool SystemPermission { get; set; }

        public virtual ICollection<EmployeeRolePermissionMap> EmployeeRolePermissionMaps
        {
            get => _employeeRolePermissionMaps ??= new List<EmployeeRolePermissionMap>();
            protected set => _employeeRolePermissionMaps = value;
        }
    }
}