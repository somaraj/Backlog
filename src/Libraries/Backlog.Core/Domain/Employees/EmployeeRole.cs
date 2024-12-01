using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Employees
{
    public class EmployeeRole : BaseEntity
    {
        private ICollection<EmployeeRolePermissionMap> _employeeRolePermissionMaps;

        public string Name { get; set; }

        public string SystemName { get; set; }

        public string? Description { get; set; }

        public bool SystemRole { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<EmployeeRolePermissionMap> EmployeeRolePermissionMaps
        {
            get => _employeeRolePermissionMaps ??= new List<EmployeeRolePermissionMap>();
            protected set => _employeeRolePermissionMaps = value;
        }
    }
}