using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Employees
{
    public interface IEmployeeService
    {
        #region Employees

        Task<IPagedList<Employee>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
            int sortColumn = -1, string sortDirection = "");

        Task<IPagedList<EmployeeRole>> GetPagedListEmployeeRoleAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
            int sortColumn = -1, string sortDirection = "");

        Task<IList<Employee>> GetAllActiveAsync();

        Task<IList<Employee>> GetAllActiveByProjectAsync(int projectId);

        Task<Employee> GetByIdAsync(int id);

        Task<Employee> GetByCodeAsync(Guid code);

        Task<Employee> GetByEmailAsync(string email);

        Task<Employee> GetByMobileNumberAsync(string mobileNo);

        Task<string> GenerateCode();

        Task InsertAsync(Employee entity);

        Task UpdateAsync(Employee entity);

        Task DeleteAsync(Employee entity);

        #endregion

        #region Employee roles

        Task AddEmployeeRoleMappingAsync(EmployeeRoleMap roleMapping);

        Task UpdateEmployeeRoleAsync(EmployeeRole employeeRole);

        Task RemoveEmployeeRoleMappingAsync(Employee employee, EmployeeRole role);

        Task DeleteEmployeeRoleAsync(EmployeeRole employeeRole);

        Task<EmployeeRole> GetEmployeeRoleByIdAsync(int employeeRoleId);

        Task<EmployeeRole> GetEmployeeRoleBySystemNameAsync(string systemName);

        Task<EmployeeRole> GetEmployeeRoleByNameAsync(string systemName);

        Task<int[]> GetEmployeeRoleIdsAsync(Employee employee, bool showHidden = false);

        Task<IList<EmployeeRole>> GetEmployeeRolesAsync(Employee employee, bool showHidden = false);

        Task<IList<EmployeeRole>> GetAllEmployeeRolesAsync(bool showHidden = false);

        Task<IList<EmployeeRolePermissionMap>> GetAllEmployeeRolePermissionMapsAsync();

        Task InsertEmployeeRoleAsync(EmployeeRole employeeRole);

        Task<bool> IsInEmployeeRoleAsync(Employee employee, string employeeRoleSystemName, bool onlyActiveRoles = true);

        Task<bool> IsAdminAsync(Employee employee, bool onlyActiveRoles = true);

        Task<bool> IsRegisteredAsync(Employee employee, bool onlyActiveRoles = true);

        #endregion

        #region Employee passwords

        Task<IList<EmployeePassword>> GetEmployeePasswordsAsync(int? employeeId = null, int? passwordsToReturn = null);

        Task<EmployeePassword> GetCurrentPasswordAsync(int employeeId);

        Task InsertEmployeePasswordAsync(EmployeePassword employeePassword);

        Task UpdateEmployeePasswordAsync(EmployeePassword employeePassword);

        #endregion

        #region Authentication/Registration

        Task<LoginResultEnum> ValidateAsync(string userName, string password);

        Task ResetPasswordAsync(int employeeId, string newPassword);

        Task<RegistrationResultEnum> RegisterAsync(Employee employee,
            Address address,
            List<int> roleIds,
            Employee loggedEmployee,
            bool emailWelcomeKit);

        Task<Employee> ValidateTokenAsync(string token);

        #endregion

        #region Projects

        Task<IList<Project>> GetAllAccessibleProjects(int employeeId);

        Task<EmployeeProjectMap> GetProjectMapping(int employeeId, int projectId);

        #endregion
    }
}