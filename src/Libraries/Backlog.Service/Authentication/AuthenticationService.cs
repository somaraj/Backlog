using System.Security.Claims;
using Backlog.Core.Caching;
using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Service.Common;
using Backlog.Service.Employees;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Backlog.Service.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields

        protected readonly IEmployeeService _employeeService;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ICacheManager _cacheManager;

        private Employee _cachedEmployee;

        #endregion

        #region Ctor

        public AuthenticationService(IEmployeeService employeeService,
            IHttpContextAccessor httpContextAccessor,
            ICacheManager cacheManager)
        {
            _employeeService = employeeService;
            _httpContextAccessor = httpContextAccessor;
            _cacheManager = cacheManager;

        }

        #endregion

        #region Methods

        public async Task SignInAsync(Employee entity, bool isPersistent)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(entity.Email))
                claims.Add(new Claim(ClaimTypes.Email, entity.Email, ClaimValueTypes.String, ServiceConstant.ClaimsIssuer));

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.Now
            };

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal, authenticationProperties);

            _cachedEmployee = entity;
        }

        public async Task SignOutAsync()
        {
            _cachedEmployee = null;
            await _cacheManager.ClearAsync();
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<Employee> GetAuthenticatedEmployeeAsync()
        {
            if (_cachedEmployee != null)
                return _cachedEmployee;

            var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return null;

            Employee employee = null;
            var claims = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email
                    && claim.Issuer.Equals(ServiceConstant.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
            if (claims != null)
                employee = await _employeeService.GetByEmailAsync(claims.Value);

            if (employee == null ||
                employee.Status != (int)EmployeeStatusEnum.Active || employee.Deleted)
                return null;

            employee.IsAdmin = await _employeeService.IsAdminAsync(employee);

            _cachedEmployee = employee;

            return _cachedEmployee;
        }

        #endregion
    }
}