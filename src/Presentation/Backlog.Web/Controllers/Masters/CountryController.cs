using AutoMapper;
using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Service.Localization;
using Backlog.Service.Logging;
using Backlog.Service.Masters;
using Backlog.Service.Security;
using Backlog.Web.Controllers.Common;
using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;
using Backlog.Web.Models.Datatable;
using Backlog.Web.Models.Masters;
using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Controllers.Masters
{
    public class CountryController : BaseController
    {
        #region Fields

        protected readonly ICountryService _countryService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public CountryController(ICountryService countryService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IMapper mapper)
        {
            _countryService = countryService;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _employeeActivityService = employeeActivityService;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        public async Task<IActionResult> Index()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCountry))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCountry))
                return AccessDenied();

            var model = new CountryModel();

            return View(model);
        }

        [HttpPost, FormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(CountryModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCountry))
                return AccessDenied();

            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Country>(model);
                await _countryService.InsertAsync(entity);

                await _employeeActivityService.InsertAsync("Country", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), entity.Name), entity);
                return continueEditing ? RedirectToAction("Edit", new { id = entity.Id }) : RedirectToAction("Index");
            }

            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Error.Failed"));

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCountry))
                return AccessDeniedPartial();

            var entity = await _countryService.GetByIdAsync(id);
            if (entity == null)
                return NoDataPartial();

            var model = _mapper.Map<CountryModel>(entity);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CountryModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCountry))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = await _countryService.GetByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);

                await _countryService.UpdateAsync(entity);

                await _employeeActivityService.InsertAsync("Country", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCountry))
                return AccessDeniedPartial();

            var entity = await _countryService.GetByIdAsync(id);
            if (entity == null)
                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.NoData,
                    Message = await _localizationService.GetResourceAsync("FormNoData.Description")
                });

            await _countryService.DeleteAsync(entity);
            await _employeeActivityService.InsertAsync("Country", string.Format(await _localizationService.GetResourceAsync("Log.RecordDeleted"), entity.Name), entity);

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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCountry))
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

            var data = await _countryService.GetPagedListAsync(search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => _mapper.Map<CountryModel>(x)),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion
    }
}