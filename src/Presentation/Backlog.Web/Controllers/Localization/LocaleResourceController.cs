using AutoMapper;
using Backlog.Core.Common;
using Backlog.Core.Domain.Localization;
using Backlog.Service.Localization;
using Backlog.Service.Logging;
using Backlog.Service.Security;
using Backlog.Web.Controllers.Common;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Common;
using Backlog.Web.Models.Datatable;
using Backlog.Web.Models.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Controllers.Localization
{
    public class LocaleResourceController : BaseController
    {
        #region Fields

        protected readonly ILocalizationService _localizationService;
        protected readonly IPermissionService _permissionService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public LocaleResourceController(ILocalizationService localizationService,
            IPermissionService permissionService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
            _localizationService = localizationService;
            _permissionService = permissionService;
            _mapper = mapper;
            _employeeActivityService = employeeActivityService;
            _workContext = workContext;
        }

        #endregion

        #region Actions

        public async Task<IActionResult> Create(int languageId)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageLocaleResource))
                return AccessDenied();

            var model = new LocaleResourceModel
            {
                LanguageId = languageId
            };

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LocaleResourceModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageLocaleResource))
                return AccessDenied();

            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<LocaleResource>(model);
                await _localizationService.InsertAsync(entity);

                await _employeeActivityService.InsertAsync("LocaleResource", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), entity.ResourceValue), entity);

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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageLocaleResource))
                return AccessDenied();

            var entity = await _localizationService.GetByIdAsync(id);
            if (entity == null)
                return RedirectToAction("Index");

            var model = _mapper.Map<LocaleResourceModel>(entity);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LocaleResourceModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageLocaleResource))
                return AccessDenied();

            if (ModelState.IsValid)
            {
                var entity = await _localizationService.GetByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);

                await _localizationService.UpdateAsync(entity);

                await _employeeActivityService.InsertAsync("LocaleResource", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.ResourceValue), entity);

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

        #endregion

        #region Data

        [HttpPost]
        public async Task<IActionResult> DataRead(DataSourceRequest request, int languageId)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageLocaleResource))
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

            var data = await _localizationService.GetPagedListAsync(languageId, search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => _mapper.Map<LocaleResourceModel>(x)),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion
    }
}