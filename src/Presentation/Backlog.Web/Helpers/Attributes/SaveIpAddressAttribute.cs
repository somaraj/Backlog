using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Data.Repository;
using Backlog.Web.Helpers.Extensions;

namespace Backlog.Web.Helpers.Attributes
{
    public sealed class SaveIpAddressAttribute : TypeFilterAttribute
    {
        #region Ctor

        public SaveIpAddressAttribute() : base(typeof(SaveIpAddressFilter))
        {
        }

        #endregion

        #region Nested filter

        private class SaveIpAddressFilter : IAsyncActionFilter
        {
            #region Fields

            protected readonly IRepository<Employee> _employeeRepository;
            protected readonly IHttpHelper _httpHelper;
            protected readonly IWorkContext _workContext;

            #endregion

            #region Ctor

            public SaveIpAddressFilter(IRepository<Employee> employeeRepository, IHttpHelper httpHelper,
                IWorkContext workContext)
            {
                _employeeRepository = employeeRepository;
                _httpHelper = httpHelper;
                _workContext = workContext;
            }

            #endregion

            #region Utilities

            private async Task SaveIpAddressAsync(ActionExecutingContext context)
            {
                ArgumentNullException.ThrowIfNull(context);

                if (!context.HttpContext.Request.IsGetRequest())
                    return;

                var currentIpAddress = _httpHelper.GetCurrentIpAddress();

                if (string.IsNullOrEmpty(currentIpAddress))
                    return;

                var employee = await _workContext.GetCurrentEmployeeAsync();
                if (!currentIpAddress.Equals(employee.LastIPAddress, StringComparison.InvariantCultureIgnoreCase))
                {
                    employee.LastIPAddress = currentIpAddress;
                    await _employeeRepository.UpdateAsync(employee);
                }
            }

            #endregion

            #region Methods

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                await SaveIpAddressAsync(context);
                if (context.Result == null)
                    await next();
            }

            #endregion
        }

        #endregion
    }
}