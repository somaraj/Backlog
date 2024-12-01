using AutoMapper;
using Backlog.Core.Common;
using Backlog.Core.Domain.Localization;
using Backlog.Service.Localization;
using Backlog.Service.Logging;
using Backlog.Service.Security;
using Backlog.Web.Controllers.Common;
using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Common;
using Backlog.Web.Models.Datatable;
using Backlog.Web.Models.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Controllers.Localization
{
    public class LanguageController : BaseController
    {
        #region Fields

        protected readonly ILanguageService _languageService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public LanguageController(ILanguageService languageService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
            _languageService = languageService;
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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageLanguage))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageLanguage))
                return AccessDenied();

            var model = new LanguageModel();

            return View(model);
        }

        [HttpPost, FormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(LanguageModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageLanguage))
                return AccessDenied();

            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Language>(model);
                await _languageService.InsertAsync(entity);

                await _employeeActivityService.InsertAsync("Language", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), entity.Name), entity);
                return continueEditing ? RedirectToAction("Edit", new { id = entity.Id }) : RedirectToAction("Index");
            }

            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Error.Failed"));

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageLanguage))
                return AccessDeniedPartial();

            var entity = await _languageService.GetByIdAsync(id);
            if (entity == null)
                return NoDataPartial();

            var model = _mapper.Map<LanguageModel>(entity);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LanguageModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageLanguage))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = await _languageService.GetByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);

                await _languageService.UpdateAsync(entity);

                await _employeeActivityService.InsertAsync("Language", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageLanguage))
                return AccessDenied();

            if (ModelState.IsValid)
            {
                var entity = await _languageService.GetByIdAsync(id);

                await _languageService.DeleteAsync(entity);

                await _employeeActivityService.InsertAsync("Language", string.Format(await _localizationService.GetResourceAsync("Log.RecordDeleted"), entity.Name), entity);

                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.Success,
                    Message = await _localizationService.GetResourceAsync("Message.DeleteSuccess")
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
        public async Task<IActionResult> DataRead(DataSourceRequest request)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageLanguage))
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

            var data = await _languageService.GetPagedListAsync(search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => _mapper.Map<LanguageModel>(x)),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion
    }
}