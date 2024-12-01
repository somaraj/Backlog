using Backlog.Core.Domain.Employees;

namespace Backlog.Service.Security
{
    public interface IPermissionService
    {
        Task<EmployeeRolePermission> GetByIdAsync(int permissionId);

        Task<EmployeeRolePermission> GetBySystemNameAsync(string systemName);

        Task<IList<EmployeeRolePermission>> GetByRoleAsync(string roleName);

        Task<IList<EmployeeRolePermission>> GetByRoleIdAsync(int employeeRoleId);

        Task<IList<EmployeeRolePermission>> GetAllEmployeeRolePermissionsAsync();

        Task InsertEmployeeRolePermissionAsync(EmployeeRolePermission entity);

        Task UpdateEmployeeRolePermissionAsync(EmployeeRolePermission entity);

        Task<bool> AuthorizeAsync(EmployeeRolePermission entity);

        Task<bool> AuthorizeAsync(EmployeeRolePermission permission, Employee user);

        Task<bool> AuthorizeAsync(string employeeRoleSystemName);

        Task<bool> AuthorizeAsync(string employeeRoleSystemName, Employee user);

        Task DeleteEmployeeRolePermissionAsync(EmployeeRolePermission permission);

        Task DeleteAllPermissionMapAsync(int employeeRoleId);
    }
}