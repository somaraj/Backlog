using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Data.Repository;
using Backlog.Web.Helpers.Extensions;

namespace Backlog.Web.Helpers.Attributes
{
    public sealed class SaveLastActivityAttribute : TypeFilterAttribute
    {
        #region Ctor

        public SaveLastActivityAttribute() : base(typeof(SaveLastActivityFilter))
        {
        }

        #endregion

        #region Nested filter

        private class SaveLastActivityFilter : IAsyncActionFilter
        {
            #region Fields

            protected readonly IRepository<Employee> _employeeRepository;
            protected readonly IWorkContext _workContext;

            #endregion

            #region Ctor

            public SaveLastActivityFilter(IRepository<Employee> employeeRepository, IWorkContext workContext)
            {
                _employeeRepository = employeeRepository;
                _workContext = workContext;
            }

            #endregion

            #region Utilities

            private async Task SaveLastActivityAsync(ActionExecutingContext context)
            {
                ArgumentNullException.ThrowIfNull(context);

                if (!context.HttpContext.Request.IsGetRequest())
                    return;

                var employee = await _workContext.GetCurrentEmployeeAsync();
                if (employee.LastActivityDate.AddMinutes(15) < DateTime.UtcNow)
                {
                    employee.LastActivityDate = DateTime.UtcNow;
                    await _employeeRepository.UpdateAsync(employee);
                }
            }

            #endregion

            #region Methods

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                await SaveLastActivityAsync(context);
                if (context.Result == null)
                    await next();
            }

            #endregion
        }

        #endregion
    }
}