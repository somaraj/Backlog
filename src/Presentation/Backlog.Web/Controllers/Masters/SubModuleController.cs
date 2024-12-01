using AutoMapper;
using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Service.Employees;
using Backlog.Service.Localization;
using Backlog.Service.Logging;
using Backlog.Service.Masters;
using Backlog.Service.Security;
using Backlog.Web.Controllers.Common;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Common;
using Backlog.Web.Models.Datatable;
using Backlog.Web.Models.Masters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Controllers.Masters
{
    public class SubModuleController : BaseController
    {
        #region Fields

        protected readonly ISubModuleService _subModuleService;
        protected readonly IModuleService _moduleService;
        protected readonly IEmployeeService _employeeService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public SubModuleController(ISubModuleService subModuleService,
            IModuleService moduleService,
            IEmployeeService employeeService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
            _subModuleService = subModuleService;
            _moduleService = moduleService;
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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageSubModule))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageSubModule))
                return AccessDeniedPartial();

            var model = new SubModuleModel();
            await InitModelAsync(model);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubModuleModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageSubModule))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<SubModule>(model);
                var employee = await _workContext.GetCurrentEmployeeAsync();

                await _subModuleService.InsertAsync(entity);

                await _employeeActivityService.InsertAsync("SubModule", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), entity.Name), entity);

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

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageSubModule))
                return AccessDeniedPartial();

            var entity = await _subModuleService.GetByIdAsync(id);
            if (entity == null)
                return NoDataPartial();

            var model = _mapper.Map<SubModuleModel>(entity);
            await InitModelAsync(model);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SubModuleModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageSubModule))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = await _subModuleService.GetByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);

                await _subModuleService.UpdateAsync(entity);

                await _employeeActivityService.InsertAsync("SubModule", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);

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
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageSubModule))
                return AccessDeniedPartial();

            var entity = await _subModuleService.GetByIdAsync(id);
            if (entity == null)
                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.NoData,
                    Message = await _localizationService.GetResourceAsync("FormNoData.Description")
                });

            await _subModuleService.DeleteAsync(entity);
            await _employeeActivityService.InsertAsync("SubModule", string.Format(await _localizationService.GetResourceAsync("Log.RecordDeleted"), entity.Name), entity);

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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageSubModule))
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

            var data = await _subModuleService.GetPagedListAsync(search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => _mapper.Map<SubModuleModel>(x)),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion

        #region Ajax

        public async Task<List<SelectListItem>> GetSubModules(int module)
        {
            var states = await _subModuleService.GetAllActiveByModuleAsync(module);
            return states.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
        }

        #endregion

        #region Helper

        private async Task InitModelAsync(SubModuleModel model)
        {
            var modules = await _moduleService.GetAllActiveAsync();
            var owners = await _employeeService.GetAllActiveAsync();

            foreach (var item in modules)
            {
                model.AvailableModules.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.ModuleId
                });
            }

            foreach (var item in owners)
            {
                model.AvailableOwners.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.OwnerId
                });
            }
        }

        #endregion
    }
}