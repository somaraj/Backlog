using Microsoft.EntityFrameworkCore;
using Backlog.Core.Caching;
using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Data.Extensions;
using Backlog.Data.Repository;
using Backlog.Service.Common;
using Backlog.Service.Employees;

namespace Backlog.Service.Security
{
    public class PermissionService : IPermissionService
    {
        #region Fields

        protected readonly ICacheManager _cacheManager;
        protected readonly IEmployeeService _employeeService;
        protected readonly IRepository<EmployeeRolePermission> _employeeRolePermissionRepository;
        protected readonly IRepository<EmployeeRolePermissionMap> _employeeRolePermissionMapRepository;
        protected readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public PermissionService(ICacheManager cacheManager,
            IEmployeeService employeeService,
            IRepository<EmployeeRolePermission> permissionRecordRepository,
            IRepository<EmployeeRolePermissionMap> employeeRolePermissionMapRepository,
            IWorkContext workContext)
        {
            _cacheManager = cacheManager;
            _employeeService = employeeService;
            _employeeRolePermissionRepository = permissionRecordRepository;
            _employeeRolePermissionMapRepository = employeeRolePermissionMapRepository;
            _workContext = workContext;
        }

        #endregion

        #region Utilities

        protected async Task<IList<EmployeeRolePermission>> GetEmployeeRolePermissionsByEmployeeRoleIdAsync(int employeeRoleId)
        {
            var key = string.Format(ServiceConstant.PermissionsAllByEmployeeRoleIdCacheKey, employeeRoleId);
            var query = from pr in _employeeRolePermissionRepository.Table
                        join urpmap in _employeeRolePermissionMapRepository.Table on pr.Id equals urpmap
                            .PermissionId
                        where urpmap.EmployeeRoleId == employeeRoleId
                        orderby pr.Id
                        select pr;

            return await _cacheManager.GetAsync(key, async () => await query.ToListAsync());
        }

        protected async Task<bool> AuthorizeAsync(string employeeRoleSystemName, int employeeRoleId)
        {
            if (string.IsNullOrEmpty(employeeRoleSystemName))
                return false;

            var key = string.Format(ServiceConstant.PermissionsAllowedCacheKey, employeeRoleId,
                employeeRoleSystemName);

            return await _cacheManager.GetAsync(key, async () =>
            {
                var permissions = await GetEmployeeRolePermissionsByEmployeeRoleIdAsync(employeeRoleId);
                foreach (var permission1 in permissions)
                    if (permission1.SystemName.Equals(employeeRoleSystemName,
                        StringComparison.InvariantCultureIgnoreCase))
                        return true;

                return false;
            });
        }

        #endregion

        #region Methods

        public async Task DeleteEmployeeRolePermissionAsync(EmployeeRolePermission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            await _employeeRolePermissionRepository.DeleteAsync(permission);

            await _cacheManager.RemoveByPrefixAsync(ServiceConstant.PermissionsPrefixCacheKey);
        }

        public async Task DeleteAllPermissionMapAsync(int employeeRoleId)
        {
            var all = await _employeeRolePermissionMapRepository.GetAllAsync(query => query.Where(x => x.EmployeeRoleId == employeeRoleId));
            await _employeeRolePermissionMapRepository.DeleteAsync(all);

            await _cacheManager.RemoveByPrefixAsync(ServiceConstant.PermissionsPrefixCacheKey);
        }

        public async Task<EmployeeRolePermission> GetByIdAsync(int permissionId)
        {
            if (permissionId == 0)
                return null;

            return await _employeeRolePermissionRepository.GetByIdAsync(permissionId);
        }

        public async Task<EmployeeRolePermission> GetBySystemNameAsync(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from pr in _employeeRolePermissionRepository.Table
                        where pr.SystemName == systemName
                        orderby pr.Id
                        select pr;

            return await query.FirstOrDefaultAsync(); ;
        }

        public async Task<IList<EmployeeRolePermission>> GetAllEmployeeRolePermissionsAsync()
        {
            var query = from pr in _employeeRolePermissionRepository.Table
                        orderby pr.Name
                        select pr;

            return await query.ToListAsync();
        }

        public async Task InsertEmployeeRolePermissionAsync(EmployeeRolePermission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            await _employeeRolePermissionRepository.InsertAsync(permission);

            await _cacheManager.RemoveByPrefixAsync(ServiceConstant.PermissionsPrefixCacheKey);
        }

        public async Task UpdateEmployeeRolePermissionAsync(EmployeeRolePermission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            await _employeeRolePermissionRepository.UpdateAsync(permission);

            await _cacheManager.RemoveByPrefixAsync(ServiceConstant.PermissionsPrefixCacheKey);
        }

        public async Task<IList<EmployeeRolePermission>> GetByRoleAsync(string roleName)
        {
            var query = _employeeRolePermissionRepository.Table.Where(x =>
                x.EmployeeRolePermissionMaps.Any(p => p.EmployeeRole.SystemName == roleName));

            return await query.ToListAsync();
        }

        public async Task<IList<EmployeeRolePermission>> GetByRoleIdAsync(int employeeRoleId)
        {
            var query = from pr in _employeeRolePermissionRepository.Table
                        join urpmap in _employeeRolePermissionMapRepository.Table on pr.Id equals urpmap
                            .PermissionId
                        where urpmap.EmployeeRoleId == employeeRoleId
                        orderby pr.Id
                        select pr;

            return await query.ToListAsync();
        }

        public async Task<bool> AuthorizeAsync(EmployeeRolePermission permission)
        {
            return await AuthorizeAsync(permission, await _workContext.GetCurrentEmployeeAsync());
        }

        public async Task<bool> AuthorizeAsync(EmployeeRolePermission permission, Employee employee)
        {
            if (permission == null)
                return false;

            if (employee == null)
                return false;

            return await AuthorizeAsync(permission.SystemName, employee);
        }

        public async Task<bool> AuthorizeAsync(string employeeRoleSystemName)
        {
            if (string.IsNullOrEmpty(employeeRoleSystemName))
                return false;

            var employee = await _workContext.GetCurrentEmployeeAsync();

            if (employee == null)
                return false;

            return await AuthorizeAsync(employeeRoleSystemName, employee);
        }

        public async Task<bool> AuthorizeAsync(string employeeRoleSystemName, Employee employee)
        {
            if (string.IsNullOrEmpty(employeeRoleSystemName))
                return false;

            var customerRoles = await _employeeService.GetEmployeeRolesAsync(employee);
            foreach (var role in customerRoles)
                if (await AuthorizeAsync(employeeRoleSystemName, role.Id))
                    return true;

            return false;
        }

        #endregion
    }
}