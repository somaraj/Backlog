using System.Data;
using AutoMapper;
using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Masters;
using Backlog.Core.Extensions;
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

namespace Backlog.Web.Controllers.Employees
{
    public class EmployeeController : BaseController
    {
        #region Fields

        protected readonly IEmployeeService _employeeService;
        protected readonly ICountryService _countryService;
        protected readonly IStateProvinceService _stateProvinceService;
        protected readonly IDocumentService _documentService;
        protected readonly IDepartmentService _departmentService;
        protected readonly IDesignationService _designationService;
        protected readonly ILanguageService _languageService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public EmployeeController(IEmployeeService employeeService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            IDocumentService documentService,
            IPermissionService permissionService,
            ILanguageService languageService,
            IDepartmentService departmentService,
            IDesignationService designationService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
            _employeeService = employeeService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _documentService = documentService;
            _departmentService = departmentService;
            _designationService = designationService;
            _permissionService = permissionService;
            _languageService = languageService;
            _localizationService = localizationService;
            _employeeActivityService = employeeActivityService;
            _workContext = workContext;
            _mapper = mapper;
        }

        #endregion

        #region Action

        public async Task<IActionResult> Index()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployee))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployee))
                return AccessDenied();

            var model = new EmployeeRegistrationModel();
            await InitModelAsync(model);

            return View(model);
        }

        [HttpPost, FormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(EmployeeRegistrationModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployee))
                return AccessDenied();

            if (ModelState.IsValid)
            {
                var loggedEmployee = await _workContext.GetCurrentEmployeeAsync();
                var employeeEntity = _mapper.Map<Employee>(model.Employee);
                var addressEntity = _mapper.Map<Address>(model.Address);

                var status = await _employeeService.RegisterAsync(employeeEntity, addressEntity, model.Employee.SelectedRoleIds,
                    loggedEmployee, model.Employee.EmailWelcomeKit);

                switch (status)
                {
                    case RegistrationResultEnum.InvalidEmpCode:
                        ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Error.UnableToRegisterEmployee"));
                        break;
                }

                if (status == RegistrationResultEnum.Successful)
                {
                    await _employeeActivityService.InsertAsync("Employee", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), employeeEntity.Name), employeeEntity);
                    return continueEditing ? RedirectToAction("Edit", new { id = employeeEntity.Id }) : RedirectToAction("Index");
                }

                await _employeeActivityService.InsertAsync("Employee", string.Format(await _localizationService.GetResourceAsync("Log.RecordError"), employeeEntity.Name), employeeEntity);
                ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Error.UnableToRegisterEmployee"));
            }

            await InitModelAsync(model);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployee))
                return AccessDenied();

            var entity = await _employeeService.GetByIdAsync(id);
            if (entity == null)
                return RedirectToAction("Index");

            var model = new EmployeeRegistrationModel
            {
                Employee = _mapper.Map<EmployeeModel>(entity),
                Address = _mapper.Map<AddressModel>(entity.Address),
            };

            model.Employee.SelectedRoleIds = entity.EmployeeEmployeeRoleMaps.Select(x => x.EmployeeRoleId).ToList();

            await InitModelAsync(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeRegistrationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployee))
                return AccessDenied();

            if (ModelState.IsValid)
            {

                var entity = await _employeeService.GetByIdAsync(model.Employee.Id);
                entity = _mapper.Map(model.Employee, entity);
                entity.Address = _mapper.Map(model.Address, entity.Address);

                #region Roles

                var avaliableRoles = await _employeeService.GetAllEmployeeRolesAsync();
                var activeRoles = avaliableRoles.Where(x => entity.EmployeeEmployeeRoleMaps.Any(y => y.EmployeeRoleId == x.Id)).ToList();
                var selectedRoles = avaliableRoles.Where(x => model.Employee.SelectedRoleIds.Any(y => y == x.Id)).ToList();
                var rolesToAdd = selectedRoles.Except(activeRoles);
                var rolesToDelete = activeRoles.Except(selectedRoles);

                foreach (var role in rolesToDelete)
                    entity.RemoveFromRole(entity.EmployeeEmployeeRoleMaps.First(x => x.EmployeeRoleId == role.Id));

                foreach (var role in rolesToAdd)
                    entity.AddToRole(new EmployeeRoleMap { EmployeeId = entity.Id, EmployeeRoleId = role.Id });

                #endregion

                await _employeeService.UpdateAsync(entity);
                await _employeeActivityService.InsertAsync("Employee", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);

                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Error.UnableToUpdateEmployee"));

            await InitModelAsync(model);

            return View(model);
        }

        public async Task<IActionResult> ResetPassword(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployee))
                return AccessDenied();

            var model = new ResetPasswordModel { Id = id };

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployee))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                await _employeeService.ResetPasswordAsync(model.Id, model.Password);

                var employee = await _employeeService.GetByIdAsync(model.Id);
                await _employeeActivityService.InsertAsync("ResetPassword", string.Format(await _localizationService.GetResourceAsync("Log.PasswordReset"), employee.Name));

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

        #endregion

        #region Data

        [HttpPost]
        public async Task<IActionResult> DataRead(DataSourceRequest request)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployee))
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

            var data = await _employeeService.GetPagedListAsync(search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => _mapper.Map<EmployeeModel>(x)),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion

        #region Helper

        private async Task InitModelAsync(EmployeeRegistrationModel model)
        {
            var countries = await _countryService.GetAllActiveAsync();
            var designations = await _designationService.GetAllActiveAsync();
            var departments = await _departmentService.GetAllActiveAsync();
            var languages = await _languageService.GetAllActiveAsync();
            var employeeRoles = await _employeeService.GetAllEmployeeRolesAsync();

            foreach (var item in countries)
            {
                model.AvailableCountries.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.Address.CountryId
                });
            }

            if (model.Address.CountryId > 0)
            {
                var states = await _stateProvinceService.GetAllActiveByCountryAsync(model.Address.CountryId.ToInt());
                foreach (var item in states)
                {
                    model.AvailableStates.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                        Selected = item.Id == model.Address.StateProvinceId
                    });
                }
            }

            foreach (var item in designations)
            {
                model.AvailableDesignations.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.Employee.DesignationId
                });
            }

            foreach (var item in departments)
            {
                model.AvailableDepartments.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.Employee.DepartmentId
                });
            }

            foreach (var item in languages)
            {
                model.AvailableLanguages.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.Employee.LanguageId
                });
            }

            foreach (var item in employeeRoles)
            {
                model.AvailableRoles.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = model.Employee.SelectedRoleIds.Contains(item.Id)
                });
            }
        }

        #endregion
    }
}