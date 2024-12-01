namespace Backlog.Core.Domain.Employees
{
    public class DefaultRolePermission
    {
        public string EmployeeRoleSystemName { get; set; }

        public IEnumerable<EmployeeRolePermission> PermissionRecords { get; set; }

        public DefaultRolePermission()
        {
            PermissionRecords = new List<EmployeeRolePermission>();
        }
    }
}