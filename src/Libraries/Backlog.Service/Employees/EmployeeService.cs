using System.Linq.Dynamic.Core;
using Backlog.Core.Caching;
using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Masters;
using Backlog.Core.Domain.Settings;
using Backlog.Data.Extensions;
using Backlog.Data.Repository;
using Backlog.Service.Common;
using Backlog.Service.Masters;
using Backlog.Service.Messages;
using Backlog.Service.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;

namespace Backlog.Service.Employees
{
    public class EmployeeService : IEmployeeService
    {
        #region Fields

        protected readonly EmployeeSettings _employeeSettings;
        protected readonly IRepository<Employee> _employeeRepository;
        protected readonly IRepository<EmployeePassword> _passwordRepository;
        protected readonly IRepository<EmployeeRole> _employeeRoleRepository;
        protected readonly IRepository<EmployeeRoleMap> _employeeRoleMapRepository;
        protected readonly IRepository<EmployeeRolePermissionMap> _permissionMapRepository;
        protected readonly IRepository<EmployeeProjectMap> _employeeProjectMapRepository;
        protected readonly IAddressService _addressService;
        protected readonly IMessageService _messageService;
        protected readonly IEncryptionService _encryptionService;
        protected readonly ISettingService _settingsService;
        protected readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public EmployeeService(EmployeeSettings employeeSettings,
            IRepository<Employee> employeeRepository,
            IRepository<EmployeePassword> passwordRepository,
            IRepository<EmployeeRole> employeeRoleRepository,
            IRepository<EmployeeRoleMap> employeeRoleMapRepository,
            IRepository<EmployeeRolePermissionMap> permissionMapRepository,
            IRepository<EmployeeProjectMap> employeeProjectMapRepository,
            IEncryptionService encryptionService,
            ISettingService settingsService,
            IAddressService addressService,
            IMessageService messageService,
            ICacheManager cacheManager)
        {
            _employeeSettings = employeeSettings;
            _employeeRepository = employeeRepository;
            _passwordRepository = passwordRepository;
            _employeeRoleRepository = employeeRoleRepository;
            _employeeRoleMapRepository = employeeRoleMapRepository;
            _permissionMapRepository = permissionMapRepository;
            _employeeProjectMapRepository = employeeProjectMapRepository;
            _encryptionService = encryptionService;
            _settingsService = settingsService;
            _addressService = addressService;
            _messageService = messageService;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Employees

        public async Task<IPagedList<Employee>> GetPagedListAsync(string search = "", int pageIndex = 0,
            int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "")
        {
            return await _employeeRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => !x.SystemAccount);
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(Employee).GetProperties();
                    var curOrderBy = propertyInfo[sortColumn].Name + " " + sortDirection;
                    query = query.OrderBy(curOrderBy);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }

                if (!string.IsNullOrWhiteSpace(search))
                    query =
                        query.Where(
                            c =>
                                c.Name.Contains(search) ||
                                c.Email.Contains(search) ||
                                c.MobileNumber.Contains(search));

                return query;
            }, pageIndex, pageSize);
        }

        public async Task<IPagedList<EmployeeRole>> GetPagedListEmployeeRoleAsync(string search = "", int pageIndex = 0,
            int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "")
        {
            return await _employeeRoleRepository.GetAllPagedAsync(query =>
            {
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(EmployeeRole).GetProperties();
                    var curOrderBy = propertyInfo[sortColumn].Name + " " + sortDirection;
                    query = query.OrderBy(curOrderBy);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }

                if (!string.IsNullOrWhiteSpace(search))
                    query =
                        query.Where(
                            c => c.Name.Contains(search) ||
                                 c.Description.Contains(search) ||
                                 c.SystemName.Contains(search));

                return query;
            }, pageIndex, pageSize);
        }

        public async Task<IList<EmployeeRolePermissionMap>> GetAllEmployeeRolePermissionMapsAsync()
        {
            return await _permissionMapRepository.GetAllAsync(includeDeleted: false);
        }

        public async Task<IList<Employee>> GetAllActiveAsync()
        {
            return await _employeeRepository.GetAllAsync(query => query.Where(x =>
                x.Status == (int)EmployeeStatusEnum.Active &&
                !x.SystemAccount), false);
        }

        public async Task<IList<Employee>> GetAllActiveByProjectAsync(int projectId)
        {
            return await _employeeProjectMapRepository.Table.Where(x =>
            (x.Employee.Status == (int)EmployeeStatusEnum.Active && !x.Employee.SystemAccount) && x.ProjectId == projectId)
                .Select(s => s.Employee).ToListAsync();
        }

        public async Task<IList<Employee>> GetAllAssigneeAsync()
        {
            return await _employeeRepository.GetAllAsync(query => query.Where(x => x.Status == (int)EmployeeStatusEnum.Active && !x.SystemAccount), false);
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task<Employee> GetByCodeAsync(Guid code)
        {
            if (code == Guid.Empty)
                return null;

            var query = from c in _employeeRepository.Table
                        where c.Code == code
                        select c;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = from c in _employeeRepository.Table
                        where c.Email == email
                        select c;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Employee> GetByMobileNumberAsync(string mobileNo)
        {
            var query = from c in _employeeRepository.Table
                        where c.MobileNumber == mobileNo
                        select c;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<string> GenerateCode()
        {
            var prefix = _employeeSettings.Prefix;
            var sequence = _employeeSettings.Sequence;

            if (string.IsNullOrEmpty(prefix))
                return null;

            return $"{prefix}{sequence + 1}";
        }

        public async Task InsertAsync(Employee entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _employeeRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Employee entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _employeeRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Employee entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.SystemAccount)
                throw new Exception($"System entity account ({entity.Email}) could not be deleted");

            await _employeeRepository.DeleteAsync(entity);
        }

        #endregion

        #region Employee roles

        public async Task AddEmployeeRoleMappingAsync(EmployeeRoleMap roleMapping)
        {
            await _employeeRoleMapRepository.InsertAsync(roleMapping);
        }

        public async Task UpdateEmployeeRoleAsync(EmployeeRole employeeRole)
        {
            await _employeeRoleRepository.UpdateAsync(employeeRole);
            await _cacheManager.RemoveAsync(ServiceConstant.EmployeeRolesAllCacheKey);
        }

        public async Task RemoveEmployeeRoleMappingAsync(Employee employee, EmployeeRole role)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            if (role is null)
                throw new ArgumentNullException(nameof(role));

            var mapping = await _employeeRoleMapRepository.Table
                .SingleOrDefaultAsync(x => x.EmployeeId == employee.Id && x.EmployeeRoleId == role.Id);

            if (mapping != null)
                await _employeeRoleMapRepository.DeleteAsync(mapping);
        }

        public async Task DeleteEmployeeRoleAsync(EmployeeRole employeeRole)
        {
            if (employeeRole == null)
                throw new ArgumentNullException(nameof(employeeRole));

            if (employeeRole.SystemRole)
                throw new Exception("System role could not be deleted");

            await _employeeRoleRepository.DeleteAsync(employeeRole);
        }

        public async Task<EmployeeRole> GetEmployeeRoleByIdAsync(int employeeRoleId)
        {
            var query = _employeeRoleRepository.Table.Where(x => x.Id == employeeRoleId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<EmployeeRole> GetEmployeeRoleBySystemNameAsync(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = _employeeRoleRepository.Table.Where(x => x.SystemName == systemName);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<EmployeeRole> GetEmployeeRoleByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = _employeeRoleRepository.Table.Where(x => x.Name == name);

            return await query.FirstOrDefaultAsync();
        }


        public async Task<int[]> GetEmployeeRoleIdsAsync(Employee employee, bool showHidden = false)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            var query = from cr in _employeeRoleRepository.Table
                        join crm in _employeeRoleMapRepository.Table on cr.Id equals crm.EmployeeRoleId
                        where crm.EmployeeId == employee.Id &&
                        (showHidden || cr.Active)
                        select cr.Id;

            return await _cacheManager.GetAsync(ServiceConstant.EmployeeRoleIdsCacheKey, () => query.ToArrayAsync());
        }

        public async Task<IList<EmployeeRole>> GetEmployeeRolesAsync(Employee employee, bool showHidden = false)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            var query = from cr in _employeeRoleRepository.Table
                        join crm in _employeeRoleMapRepository.Table on cr.Id equals crm.EmployeeRoleId
                        where crm.EmployeeId == employee.Id &&
                        (showHidden || cr.Active)
                        select cr;

            return await query.ToListAsync();
        }

        public async Task<IList<EmployeeRole>> GetAllEmployeeRolesAsync(bool showHidden = false)
        {
            var query = from cr in _employeeRoleRepository.Table
                        orderby cr.Name
                        where showHidden || cr.Active
                        select cr;

            var employeeRoles = await _cacheManager.GetAsync(ServiceConstant.EmployeeRolesAllCacheKey, () => query.ToListAsync());

            return employeeRoles;
        }

        public async Task InsertEmployeeRoleAsync(EmployeeRole employeeRole)
        {
            await _employeeRoleRepository.InsertAsync(employeeRole);

            if (employeeRole.Id > 0)
                await _cacheManager.RemoveAsync(ServiceConstant.EmployeeRolesAllCacheKey);
        }

        public async Task<bool> IsInEmployeeRoleAsync(Employee employee,
            string employeeRoleSystemName, bool onlyActiveEmployeeRoles = true)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (string.IsNullOrEmpty(employeeRoleSystemName))
                throw new ArgumentNullException(nameof(employeeRoleSystemName));

            var employeeRoles = await GetEmployeeRolesAsync(employee, !onlyActiveEmployeeRoles);

            return employeeRoles?.Any(cr => cr.SystemName == employeeRoleSystemName) ?? false;
        }

        public async Task<bool> IsAdminAsync(Employee employee, bool onlyActiveEmployeeRoles = true)
        {
            return await IsInEmployeeRoleAsync(employee, EmployeeConstant.AdministratorsRoleName, onlyActiveEmployeeRoles);
        }

        public async Task<bool> IsRegisteredAsync(Employee employee, bool onlyActiveEmployeeRoles = true)
        {
            return await IsInEmployeeRoleAsync(employee, EmployeeConstant.RegisteredRoleName, onlyActiveEmployeeRoles);
        }

        #endregion

        #region Employee passwords

        public async Task<IList<EmployeePassword>> GetEmployeePasswordsAsync(int? employeeId = null, int? passwordsToReturn = null)
        {
            var query = _passwordRepository.Table;

            //filter by employee
            if (employeeId.HasValue)
                query = query.Where(password => password.EmployeeId == employeeId.Value);

            if (passwordsToReturn.HasValue)
                query = query.OrderByDescending(password => password.CreatedOn).Take(passwordsToReturn.Value);

            return await query.ToListAsync();
        }

        public async Task<EmployeePassword> GetCurrentPasswordAsync(int employeeId)
        {
            if (employeeId == 0)
                return null;

            var currPassword = await GetEmployeePasswordsAsync(employeeId, passwordsToReturn: 1);
            return currPassword.FirstOrDefault();
        }

        public async Task InsertEmployeePasswordAsync(EmployeePassword employeePassword)
        {
            await _passwordRepository.InsertAsync(employeePassword);
        }

        public async Task UpdateEmployeePasswordAsync(EmployeePassword employeePassword)
        {
            await _passwordRepository.UpdateAsync(employeePassword);
        }

        #endregion

        #region Authentication/Registration

        public async Task<LoginResultEnum> ValidateAsync(string email, string password)
        {
            var entity = await GetByEmailAsync(email);
            if (entity == null)
                return LoginResultEnum.NotExist;
            if (entity.Deleted)
                return LoginResultEnum.Deleted;
            if (entity.Status != (int)EmployeeStatusEnum.Active)
                return LoginResultEnum.NotActive;

            var activePassword = await GetCurrentPasswordAsync(entity.Id);

            if (activePassword == null)
                return LoginResultEnum.LockedOut;

            var enteredPassword = _encryptionService.CreatePasswordHash(password, activePassword.PasswordSalt);
            if (!activePassword.Password.Equals(enteredPassword))
                return LoginResultEnum.WrongPassword;

            if (!await IsRegisteredAsync(entity))
                return LoginResultEnum.NotRegistered;

            return LoginResultEnum.Successful;
        }

        public async Task ResetPasswordAsync(int employeeId, string newPassword)
        {
            var entity = await GetByIdAsync(employeeId);

            var newEntity = new EmployeePassword
            {
                Employee = entity,
                CreatedOn = DateTime.Now
            };

            var saltKey = _encryptionService.CreateSaltKey(10);
            var encryptedNewPassword = _encryptionService.CreatePasswordHash(newPassword, saltKey);

            var currentPassword = await GetCurrentPasswordAsync(entity.Id);
            if (currentPassword != null)
            {
                currentPassword.PasswordSalt = saltKey;
                currentPassword.Password = encryptedNewPassword;
                await UpdateEmployeePasswordAsync(currentPassword);
            }
            else
            {
                newEntity.PasswordSalt = saltKey;
                newEntity.Password = encryptedNewPassword;
                await InsertEmployeePasswordAsync(newEntity);
            }
        }

        public async Task<RegistrationResultEnum> RegisterAsync(Employee employee,
            Address address,
            List<int> roleIds,
            Employee loggedEmployee,
            bool emailWelcomeKit)
        {
            var newAddressEntity = await _addressService.InsertAsync(address);
            if (newAddressEntity.Id > 0)
            {
                employee.Code = Guid.NewGuid();
                employee.Status = (int)EmployeeStatusEnum.Active;
                employee.LanguageId = loggedEmployee.LanguageId;
                employee.AddressId = newAddressEntity.Id;
                employee.SystemAccount = false;
                employee.Deleted = false;

                await InsertAsync(employee);
                var newEmployeeEntity = await GetByEmailAsync(employee.Email);
                if (newEmployeeEntity != null && newEmployeeEntity.Id > 0)
                {
                    var registeredRole = await GetEmployeeRoleBySystemNameAsync(EmployeeConstant.RegisteredRoleName);

                    if (registeredRole == null)
                        throw new Exception("Registered role not found");

                    if (!roleIds.Any(x => x == registeredRole.Id))
                        newEmployeeEntity.AddToRole(new EmployeeRoleMap { EmployeeRole = registeredRole });

                    foreach (var roleId in roleIds)
                    {
                        newEmployeeEntity.AddToRole(new EmployeeRoleMap { EmployeeId = newEmployeeEntity.Id, EmployeeRoleId = roleId });
                    }

                    await UpdateAsync(newEmployeeEntity);

                    if (emailWelcomeKit)
                    {
                        await _messageService.EmailActivationKitAsync(newEmployeeEntity);
                    }

                    return RegistrationResultEnum.Successful;
                }
            }

            return RegistrationResultEnum.Failed;
        }

        public async Task<Employee> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                var decryptedToken = _encryptionService.DecryptText(token);
                var tokenArr = decryptedToken.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokenArr.Length != 2)
                    return null;

                if (Guid.TryParse(tokenArr[0].ToString(), out Guid code))
                {
                    var employee = await GetByCodeAsync(code);
                    if (employee == null || employee.Id <= 0 || employee.Deleted || employee.Status != (int)EmployeeStatusEnum.Active)
                        return null;

                    var isValidToken = _encryptionService.ValidateToken(tokenArr[1].ToString(), 10);
                    if (!isValidToken)
                        return null;

                    return employee;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Projects

        public async Task<IList<Project>> GetAllAccessibleProjects(int employeeId)
        {
            var query = _employeeProjectMapRepository.Table
                .Where(x => x.Project.Active && !x.Project.Deleted && x.EmployeeId == employeeId)
                .Select(s => s.Project);

            return await _cacheManager.GetAsync(ServiceConstant.AccessibleProjectCacheKey, () => query.ToListAsync());
        }

        public async Task<EmployeeProjectMap> GetProjectMapping(int employeeId, int projectId)
        {
            var query = _employeeProjectMapRepository.Table
                .Where(x => x.Project.Active && !x.Project.Deleted && x.EmployeeId == employeeId && x.ProjectId == projectId);

            return await query.FirstOrDefaultAsync();
        }

        #endregion
    }
}