using AutoMapper;
using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
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

namespace Backlog.Web.Controllers.Masters
{
    public class StatusController : BaseController
    {
        #region Fields

        protected readonly IStatusService _statusService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public StatusController(IStatusService statusService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStatus))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStatus))
                return AccessDeniedPartial();

            var model = new StatusModel();

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StatusModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStatus))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Status>(model);

                await _statusService.InsertAsync(entity);
                await _employeeActivityService.InsertAsync("Status", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), entity.Name), entity);

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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStatus))
                return AccessDeniedPartial();

            var entity = await _statusService.GetByIdAsync(id);
            if (entity == null)
                return NoDataPartial();

            var model = _mapper.Map<StatusModel>(entity);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StatusModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStatus))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = await _statusService.GetByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);

                await _statusService.UpdateAsync(entity);

                await _employeeActivityService.InsertAsync("Status", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);

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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStatus))
                return AccessDeniedPartial();

            var entity = await _statusService.GetByIdAsync(id);
            if (entity == null)
                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.NoData,
                    Message = await _localizationService.GetResourceAsync("FormNoData.Description")
                });

            await _statusService.DeleteAsync(entity);
            await _employeeActivityService.InsertAsync("Status", string.Format(await _localizationService.GetResourceAsync("Log.RecordDeleted"), entity.Name), entity);

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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStatus))
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

            var data = await _statusService.GetPagedListAsync(search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => new StatusModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    GroupName = ((StatusGroupEnum)x.GroupId).ToString(),
                    TextColor = x.TextColor,
                    BackgroundColor = x.BackgroundColor,
                    IconClass = x.IconClass,
                    SystemDefined = x.SystemDefined,
                    Active = x.Active
                }),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion
    }
}