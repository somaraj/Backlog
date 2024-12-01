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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Controllers.Masters
{
    public class StateProvinceController : BaseController
    {
        #region Fields

        protected readonly IStateProvinceService _stateProvinceService;
        protected readonly IPermissionService _permissionService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public StateProvinceController(IStateProvinceService stateProvinceService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IMapper mapper)
        {
            _stateProvinceService = stateProvinceService;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _employeeActivityService = employeeActivityService;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        public async Task<IActionResult> Index()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStateProvince))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Create(int countryId)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStateProvince))
                return AccessDeniedPartial();

            var model = new StateProvinceModel
            {
                CountryId = countryId
            };

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StateProvinceModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStateProvince))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<StateProvince>(model);
                await _stateProvinceService.InsertAsync(entity);

                await _employeeActivityService.InsertAsync("StateProvince", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), entity.Name), entity);

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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStateProvince))
                return AccessDeniedPartial();

            var entity = await _stateProvinceService.GetByIdAsync(id);
            if (entity == null)
                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.NoData,
                    Message = await _localizationService.GetResourceAsync("FormNoData.Description")
                });

            var model = _mapper.Map<StateProvinceModel>(entity);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StateProvinceModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStateProvince))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = await _stateProvinceService.GetByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);

                await _stateProvinceService.UpdateAsync(entity);

                await _employeeActivityService.InsertAsync("StateProvince", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);

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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCountry))
                return AccessDeniedPartial();

            var entity = await _stateProvinceService.GetByIdAsync(id);
            if (entity == null)
                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.NoData,
                    Message = await _localizationService.GetResourceAsync("FormNoData.Description")
                });

            await _stateProvinceService.DeleteAsync(entity);
            await _employeeActivityService.InsertAsync("StateProvince", string.Format(await _localizationService.GetResourceAsync("Log.RecordDeleted"), entity.Name), entity);

            return Json(new JsonResponseModel
            {
                Status = HttpStatusCodeEnum.Success,
                Message = await _localizationService.GetResourceAsync("Message.DeleteSuccess")
            });
        }

        #endregion

        #region Data

        [HttpPost]
        public async Task<IActionResult> DataRead(DataSourceRequest request, int countryId)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageStateProvince))
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

            var data = await _stateProvinceService.GetPagedListAsync(countryId, search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => _mapper.Map<StateProvinceModel>(x)),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion

        #region Ajax

        public async Task<List<SelectListItem>> GetStates(int country)
        {
            var states = await _stateProvinceService.GetAllActiveByCountryAsync(country);
            return states.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
        }

        #endregion
    }
}