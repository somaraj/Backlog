using Backlog.Core.Common;
using Backlog.Service.Employees;
using Backlog.Service.Masters;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Masters;
using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Components
{
    public class ProjectSelector : ViewComponent
    {
        protected readonly IWorkContext _workContext;
        protected readonly IEmployeeService _employeeService;
        protected readonly ISettingService _settingsService;

        public ProjectSelector(IWorkContext workContext,
            IEmployeeService employeeService,
            ISettingService settingsService)
        {
            _workContext = workContext;
            _employeeService = employeeService;
            _settingsService = settingsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var employee = await _workContext.GetCurrentEmployeeAsync();
            var menusEntities = await _employeeService.GetAllAccessibleProjects(employee.Id);
            var projectId = await HttpContext.Session.GetAsync<int>(Constant.ActiveProjectSession);

            var model = menusEntities.Select(x => new ProjectSelectorModel
            {
                Id = x.Id,
                Name = x.Name,
                Selected = x.Id == projectId
            }).ToList();

            return View(model);
        }
    }
}