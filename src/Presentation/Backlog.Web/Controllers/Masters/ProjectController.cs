using AutoMapper;
using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Masters;
using Backlog.Service.Employees;
using Backlog.Service.Localization;
using Backlog.Service.Logging;
using Backlog.Service.Masters;
using Backlog.Service.Security;
using Backlog.Web.Controllers.Common;
using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Common;
using Backlog.Web.Models.Datatable;
using Backlog.Web.Models.Employees;
using Backlog.Web.Models.Masters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Controllers.Masters
{
    public class ProjectController : BaseController
    {
        #region Fields

        protected readonly IProjectService _projectService;
        protected readonly IClientService _clientService;
        protected readonly IEmployeeService _employeeService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public ProjectController(IProjectService projectService,
            IClientService clientService,
            IEmployeeService employeeService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
            _projectService = projectService;
            _clientService = clientService;
            _employeeService = employeeService;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _employeeActivityService = employeeActivityService;
            _workContext = workContext;
            _mapper = mapper;

        }

        #endregion

        #region Actions

        public async Task<IActionResult> Index()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDeniedPartial();

            var model = new ProjectModel();
            await InitModelAsync(model);

            return View(model);
        }

        [HttpPost, FormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(ProjectModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Project>(model);
                var employee = await _workContext.GetCurrentEmployeeAsync();

                await _projectService.InsertAsync(entity);

                await _employeeActivityService.InsertAsync("Project", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), entity.Name), entity);
                return continueEditing ? RedirectToAction("Edit", new { id = entity.Id }) : RedirectToAction("Index");
            }

            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Error.Failed"));

            await InitModelAsync(model);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDeniedPartial();

            var entity = await _projectService.GetByIdAsync(id);
            if (entity == null)
                return NoDataPartial();

            var model = _mapper.Map<ProjectModel>(entity);
            await InitModelAsync(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = await _projectService.GetByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);

                await _projectService.UpdateAsync(entity);

                await _employeeActivityService.InsertAsync("Project", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDeniedPartial();

            var entity = await _projectService.GetByIdAsync(id);
            if (entity == null)
                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.NoData,
                    Message = await _localizationService.GetResourceAsync("FormNoData.Description")
                });

            await _projectService.DeleteAsync(entity);
            await _employeeActivityService.InsertAsync("Project", string.Format(await _localizationService.GetResourceAsync("Log.RecordDeleted"), entity.Name), entity);

            return Json(new JsonResponseModel
            {
                Status = HttpStatusCodeEnum.Success,
                Message = await _localizationService.GetResourceAsync("Message.DeleteSuccess")
            });
        }

        #endregion

        #region Member Action

        public async Task<IActionResult> AddMember(int projectId)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDeniedPartial();

            var model = new EmployeeProjectModel
            {
                ProjectId = projectId
            };

            await InitMemberModelAsync(model);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(EmployeeProjectModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = new EmployeeProjectMap
                {
                    EmployeeId = model.EmployeeId,
                    ProjectId = model.ProjectId
                };
                var employee = await _employeeService.GetByIdAsync(model.EmployeeId);

                await _projectService.InsertMemberAsync(entity);

                await _employeeActivityService.InsertAsync("ProjectMember", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), employee.Name), entity);

                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.Success,
                    Message = await _localizationService.GetResourceAsync("Message.SaveSuccess")
                });
            }

            return Json(new JsonResponseModel
            {
                Status = ModelState.IsValid ? HttpStatusCodeEnum.InternalServerError : HttpStatusCodeEnum.ValidationError,
                Message = await _localizationService.GetResourceAsync("Error.Failed"),
                Errors = ModelState.AllErrors()
            });
        }

        public async Task<IActionResult> EditMember(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDeniedPartial();

            var entity = await _projectService.GetMemberByIdAsync(id);
            if (entity == null)
                return NoDataPartial();

            var model = _mapper.Map<EmployeeProjectModel>(entity);
            await InitMemberModelAsync(model);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember(EmployeeProjectModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = await _projectService.GetMemberByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);

                await _projectService.UpdateMemberAsync(entity);

                await _employeeActivityService.InsertAsync("ProjectMember", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Employee.Name), entity);

                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.Success,
                    Message = await _localizationService.GetResourceAsync("Message.UpdateSuccess")
                });
            }

            return Json(new JsonResponseModel
            {
                Status = ModelState.IsValid ? HttpStatusCodeEnum.InternalServerError : HttpStatusCodeEnum.ValidationError,
                Message = await _localizationService.GetResourceAsync("Error.Failed"),
                Errors = ModelState.AllErrors()
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMember(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDeniedPartial();

            var entity = await _projectService.GetMemberByIdAsync(id);
            var tempEntity = entity;
            if (entity == null)
                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.NoData,
                    Message = await _localizationService.GetResourceAsync("FormNoData.Description")
                });

            await _projectService.DeleteMemberAsync(entity);
            await _employeeActivityService.InsertAsync("ProjectMember", string.Format(await _localizationService.GetResourceAsync("Log.RecordDeleted"), tempEntity.Employee.Name), tempEntity);

            return Json(new JsonResponseModel
            {
                Status = HttpStatusCodeEnum.Success,
                Message = await _localizationService.GetResourceAsync("Message.DeleteSuccess")
            });
        }

        #endregion

        #region Data

        [HttpPost]
        public async Task<IActionResult> DataRead(DataSourceRequest request)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDeniedDataRead();

            var sortColumn = -1;
            var sortDirection = "asc";
            var search = string.Empty;

            if (!string.IsNullOrEmpty(Request.Form["order[0][column]"]))
            {
                sortColumn = int.Parse(Request.Form["order[0][column]"]);
            }

            if (!string.IsNullOrEmpty(Request.Form["order[0][dir]"]))
            {
                sortDirection = Request.Form["order[0][dir]"];
            }

            if (!string.IsNullOrEmpty(Request.Form["search[value]"]))
            {
                search = Request.Form["search[value]"];
            }

            var data = await _projectService.GetPagedListAsync(search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => _mapper.Map<ProjectGridModel>(x)),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        [HttpPost]
        public async Task<IActionResult> DataReadMember(int projectId, DataSourceRequest request)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageProject))
                return AccessDeniedDataRead();

            var sortColumn = -1;
            var sortDirection = "asc";
            var search = string.Empty;

            if (!string.IsNullOrEmpty(Request.Form["order[0][column]"]))
            {
                sortColumn = int.Parse(Request.Form["order[0][column]"]);
            }

            if (!string.IsNullOrEmpty(Request.Form["order[0][dir]"]))
            {
                sortDirection = Request.Form["order[0][dir]"];
            }

            if (!string.IsNullOrEmpty(Request.Form["search[value]"]))
            {
                search = Request.Form["search[value]"];
            }

            var data = await _projectService.GetPagedListMembersAsync(projectId, search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => _mapper.Map<EmployeeProjectModel>(x)),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion

        #region Helper

        private async Task InitModelAsync(ProjectModel model)
        {
            var clients = await _clientService.GetAllActiveAsync();

            foreach (var item in clients)
            {
                model.AvailableClients.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.ClientId
                });
            }
        }

        private async Task InitMemberModelAsync(EmployeeProjectModel model)
        {
            var employees = await _employeeService.GetAllActiveAsync();

            foreach (var item in employees)
            {
                model.AvailableEmployees.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.EmployeeId
                });
            }
        }

        #endregion
    }
}