using Backlog.Core.Domain.Employees;

namespace Backlog.Service.Security
{
    public interface IPermissionProvider
    {
        IEnumerable<EmployeeRolePermission> GetPermissions();
    }
}
