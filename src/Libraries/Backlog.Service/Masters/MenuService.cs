using Microsoft.EntityFrameworkCore;
using Backlog.Core.Caching;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Extensions;
using Backlog.Data.Repository;
using Backlog.Service.Common;

namespace Backlog.Service.Masters
{
    public class MenuService : IMenuService
    {
        #region Fields

        protected readonly IRepository<Menu> _menuRepository;
        protected readonly IRepository<EmployeeRolePermission> _repositoryEmployeeRolePermission;
        protected readonly IRepository<EmployeeRolePermissionMap> _repositoryEmployeeRolePermissionMap;
        protected readonly IRepository<EmployeeRoleMap> _repositoryEmployeeRoleMap;
        protected readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public MenuService(IRepository<Menu> menuRepository,
            ICacheManager cacheManager,
            IRepository<EmployeeRolePermission> repositoryEmployeeRolePermission,
            IRepository<EmployeeRolePermissionMap> repositoryEmployeeRolePermissionMap,
            IRepository<EmployeeRoleMap> repositoryEmployeeRoleMap)
        {
            _menuRepository = menuRepository;
            _cacheManager = cacheManager;
            _repositoryEmployeeRolePermission = repositoryEmployeeRolePermission;
            _repositoryEmployeeRolePermissionMap = repositoryEmployeeRolePermissionMap;
            _repositoryEmployeeRoleMap = repositoryEmployeeRoleMap;
        }

        #endregion

        #region Methods

        public async Task<IList<Menu>> GetAllAsync()
        {
            return await _menuRepository.GetAllAsync(includeDeleted: false);
        }

        public async Task<IList<Menu>> GetAllAsync(Employee employee)
        {
            var key = ServiceConstant.MenuCacheKey;
            var query = (from menu in _menuRepository.Table
                         join rolePermission in _repositoryEmployeeRolePermission.Table on menu.Permission equals rolePermission.SystemName
                         join rolePermissionMap in _repositoryEmployeeRolePermissionMap.Table on rolePermission.Id equals rolePermissionMap.PermissionId
                         join employeeRoleMap in _repositoryEmployeeRoleMap.Table on rolePermissionMap.EmployeeRoleId equals employeeRoleMap.EmployeeRoleId
                         where menu.Active && employeeRoleMap.EmployeeId == employee.Id
                         orderby menu.DisplayOrder
                         select menu).Distinct();

            return await _cacheManager.GetAsync(key, async () => await query.ToListAsync());
        }

        #endregion
    }
}