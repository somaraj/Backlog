using AutoMapper;
using Backlog.Core.Common;
using Backlog.Core.Domain.WorkItems;
using Backlog.Core.Extensions;
using Backlog.Service.Employees;
using Backlog.Service.Localization;
using Backlog.Service.Logging;
using Backlog.Service.Masters;
using Backlog.Service.Security;
using Backlog.Service.WorkItems;
using Backlog.Web.Controllers.Common;
using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Datatable;
using Backlog.Web.Models.Masters;
using Backlog.Web.Models.WorkItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Backlog.Web.Controllers.WorkItems
{
    public class BacklogController : BaseController
    {
        #region Fields

        protected readonly IBacklogItemService _backlogItemService;
        protected readonly IEmployeeService _employeeService;
        protected readonly ITaskTypeService _taskTypeService;
        protected readonly IReporterService _reporterService;
        protected readonly ISeverityService _severityService;
        protected readonly IModuleService _moduleService;
        protected readonly ISubModuleService _subModuleService;
        protected readonly IStatusService _statusService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public BacklogController(IBacklogItemService backlogItemService,
            IEmployeeService employeeService,
            ITaskTypeService taskTypeService,
            IReporterService reporterService,
            ISeverityService severityService,
            IModuleService moduleService,
            ISubModuleService subModuleService,
            IStatusService statusService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
            _backlogItemService = backlogItemService;
            _employeeService = employeeService;
            _taskTypeService = taskTypeService;
            _reporterService = reporterService;
            _severityService = severityService;
            _moduleService = moduleService;
            _subModuleService = subModuleService;
            _statusService = statusService;
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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBacklog))
                return AccessDenied();

            var loggedEmployee = await _workContext.GetCurrentEmployeeAsync();
            var projectId = await HttpContext.Session.GetAsync<int>(Constant.ActiveProjectSession);
            var projectMap = await _employeeService.GetProjectMapping(loggedEmployee.Id, projectId);

            var model = await GetProjectAccess();

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBacklog))
                return AccessDenied();

            var projectAccess = await GetProjectAccess();
            if (projectAccess == null || !projectAccess.CanReport && !projectAccess.CanClose)
                return AccessDenied();

            var model = new BacklogItemModel();
            await InitModelAsync(model);

            return View(model);
        }

        [HttpPost, FormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(BacklogItemModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBacklog))
                return AccessDenied();

            if (ModelState.IsValid)
            {
                var projectId = await GetProjectIdFromSession();
                var loggedEmployee = await _workContext.GetCurrentEmployeeAsync();

                var entity = _mapper.Map<BacklogItem>(model);
                entity.ProjectId = projectId;
                entity.CreatedById = loggedEmployee.Id;
                entity.CreatedOn = DateTime.Now;

                var documentIds = new List<int>();
                if (!string.IsNullOrEmpty(model.Token))
                    documentIds = JsonConvert.DeserializeObject<List<int>>(model.Token);

                await _backlogItemService.InsertAsync(entity, documentIds);

                if (entity.Id > 0)
                {
                    await _employeeActivityService.InsertAsync("BackLog", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), entity.Code), entity);
                    return continueEditing ? RedirectToAction("Edit", new { id = entity.Id }) : RedirectToAction("Index");
                }

                await _employeeActivityService.InsertAsync("BackLog", string.Format(await _localizationService.GetResourceAsync("Log.RecordError"), entity.Code), entity);
                ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Error.UnableToCreateTask"));
            }

            ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Error.UnableToCreateTask"));

            await InitModelAsync(model);

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBacklog))
                return AccessDenied();

            var projectAccess = await GetProjectAccess();
            if (projectAccess == null || !projectAccess.CanReport && !projectAccess.CanClose)
                return AccessDenied();

            var entity = await _backlogItemService.GetByIdAsync(id);
            if (entity == null)
                return RedirectToAction("Index");

            var model = _mapper.Map<BacklogItemModel>(entity);

            await InitModelAsync(model);

            return View(model);
        }

        #endregion

        #region Actions For Comments

        public async Task<IActionResult> Comments(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBacklog))
                return AccessDeniedPartial();

            var projectAccess = await GetProjectAccess();
            if (projectAccess == null || !projectAccess.CanComment)
                return AccessDeniedPartial();

            return PartialView();
        }

        #endregion

        #region Data

        [HttpPost]
        public async Task<IActionResult> DataRead(DataSourceRequest request)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBacklog))
                return AccessDeniedDataRead();

            var projectId = await HttpContext.Session.GetAsync<int>(Constant.ActiveProjectSession);

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

            var data = await _backlogItemService.GetPagedListAsync(projectId, search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => new BacklogItemGridModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    DueDate = x.DueDate,
                    Project = x.Project.Name,
                    Module = x.Module != null ? x.Module.Name : "-",
                    SubModule = x.SubModule != null ? x.SubModule.Name : "-",
                    Sprint = x.Sprint?.Name,
                    Assignee = x.Assignee != null ? x.Assignee.Name : "Unassigned",
                    ReOpenCount = x.ReOpenCount,
                    SubTaskCount = x.SubTaskCount,
                    TaskType = _mapper.Map<TaskTypeModel>(x.TaskType),
                    Severity = _mapper.Map<SeverityModel>(x.Severity),
                    Status = _mapper.Map<StatusModel>(x.Status)
                }),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion

        #region Helper

        private async Task<BacklogModel> GetProjectAccess()
        {
            var loggedEmployee = await _workContext.GetCurrentEmployeeAsync();
            var projectId = await HttpContext.Session.GetAsync<int>(Constant.ActiveProjectSession);
            var projectMap = await _employeeService.GetProjectMapping(loggedEmployee.Id, projectId);

            return new BacklogModel
            {
                ProjectId = projectMap != null ? projectMap.Id : 0,
                CanReport = projectMap != null ? projectMap.CanReport : false,
                CanClose = projectMap != null ? projectMap.CanClose : false
            };
        }

        private async Task InitModelAsync(BacklogItemModel model)
        {
            var projectId = await GetProjectIdFromSession();
            var taskTypes = await _taskTypeService.GetAllActiveAsync();
            var reporters = await _reporterService.GetAllActiveAsync();
            var severities = await _severityService.GetAllActiveAsync();
            var modules = await _moduleService.GetAllActiveByProjectAsync(projectId);
            var assignees = await _employeeService.GetAllActiveByProjectAsync(projectId);
            var status = await _statusService.GetAllActiveAsync();

            foreach (var item in taskTypes)
            {
                model.AvailableTaskTypes.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.TaskTypeId
                });
            }

            foreach (var item in reporters)
            {
                model.AvailableReporters.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.ReporterId
                });
            }

            foreach (var item in severities)
            {
                model.AvailableSeverities.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.SeverityId
                });
            }

            foreach (var item in modules)
            {
                model.AvailableModules.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.ModuleId
                });
            }

            if (model.ModuleId > 0)
            {
                var subModules = await _subModuleService.GetAllActiveByModuleAsync(model.ModuleId.ToInt());
                foreach (var item in subModules)
                {
                    model.AvailableSubModules.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                        Selected = item.Id == model.SubModuleId
                    });
                }
            }

            foreach (var item in assignees)
            {
                model.AvailableAssignees.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.AssigneeId
                });
            }

            var filteredStatus = status.Where(x => x.GroupId != (int)StatusGroupEnum.Closed
                && x.GroupId != (int)StatusGroupEnum.ReOpened);

            var newStatus = filteredStatus.FirstOrDefault(x => x.GroupId == (int)StatusGroupEnum.New);

            if (model.Id == 0)
            {
                if (newStatus != null)
                {
                    model.AvailableStatus.Add(new SelectListItem
                    {
                        Text = newStatus.Name,
                        Value = newStatus.Id.ToString(),
                        Selected = true
                    });
                }
            }
            else
            {
                foreach (var item in filteredStatus)
                {
                    model.AvailableStatus.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                        Selected = item.Id == model.StatusId
                    });
                }
            }

            if (model.Id > 0)
            {
                var curStatus = status.FirstOrDefault(x => x.Id == model.StatusId);
                if (curStatus != null)
                {
                    switch (curStatus.GroupId)
                    {
                        case (int)StatusGroupEnum.New:
                            model.EditMode = 1;
                            break;
                        case (int)StatusGroupEnum.Closed:
                            model.EditMode = 2;
                            break;
                        case (int)StatusGroupEnum.ReOpened:
                            model.EditMode = 3;
                            break;
                    }
                }
            }
        }

        #endregion
    }
}