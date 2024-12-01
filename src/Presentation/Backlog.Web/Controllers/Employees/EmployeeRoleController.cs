using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Service.Employees;
using Backlog.Service.Localization;
using Backlog.Service.Logging;
using Backlog.Service.Security;
using Backlog.Web.Controllers.Common;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Common;
using Backlog.Web.Models.Datatable;
using Backlog.Web.Models.Employees;

namespace Backlog.Web.Controllers.Employees
{
    public class EmployeeRoleController : BaseController
    {
        #region Fields

        protected readonly IEmployeeService _employeeService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public EmployeeRoleController(IEmployeeService employeeService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployeeRole))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployeeRole))
                return AccessDenied();

            var model = new EmployeeRoleModel();

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeRoleModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployeeRole))
                return AccessDenied();

            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<EmployeeRole>(model);
                var employee = await _workContext.GetCurrentEmployeeAsync();

                await _employeeService.InsertEmployeeRoleAsync(entity);

                await _employeeActivityService.InsertAsync("EmployeeRole", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), entity.Name), entity);

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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployeeRole))
                return AccessDenied();

            var entity = await _employeeService.GetEmployeeRoleByIdAsync(id);
            if (entity == null)
                return RedirectToAction("Index");

            var model = _mapper.Map<EmployeeRoleModel>(entity);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeRoleModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployeeRole))
                return AccessDenied();

            if (ModelState.IsValid)
            {
                var entity = await _employeeService.GetEmployeeRoleByIdAsync(model.Id);
                if (entity.SystemRole)
                {
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                }
                else
                {
                    entity = _mapper.Map(model, entity);
                }

                await _employeeService.UpdateEmployeeRoleAsync(entity);

                await _employeeActivityService.InsertAsync("EmployeeRole", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);

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

        public async Task<IActionResult> Permission(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployeeRole))
                return AccessDenied();

            var loggedEmployee = await _workContext.GetCurrentEmployeeAsync();
            var permissions = await _permissionService.GetAllEmployeeRolePermissionsAsync();
            var permissionMaps = await _employeeService.GetAllEmployeeRolePermissionMapsAsync();
            var role = await _employeeService.GetEmployeeRoleByIdAsync(id);
            var model = new EmployeeRolePermissionGridModel
            {
                RoleId = id,
                SystemRole = role.SystemRole,
                IsAdmin = loggedEmployee.IsAdmin,
                EmployeeRolePermission = permissions.Select(x => _mapper.Map<EmployeeRolePermissionModel>(x)).ToList(),
                EmployeeRolePermissionMap = permissionMaps.Select(x => _mapper.Map<EmployeeRolePermissionMapModel>(x)).ToList()
            };

            return PartialView(model);
        }

        #endregion

        #region Data

        [HttpPost]
        public async Task<IActionResult> DataRead(DataSourceRequest request)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployeeRole))
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

            var data = await _employeeService.GetPagedListEmployeeRoleAsync(search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => _mapper.Map<EmployeeRoleModel>(x)),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion

        #region Ajax

        [HttpPost]
        public async Task<bool> SetPermission(int role, int permission, bool flag)
        {
            var rolePermission = await _permissionService.GetByIdAsync(permission);
            var roles = await _employeeService.GetAllEmployeeRolesAsync(true);

            if (flag)
            {
                rolePermission.EmployeeRolePermissionMaps.Add(new EmployeeRolePermissionMap
                {
                    PermissionId = permission,
                    EmployeeRoleId = role
                });

                await _permissionService.UpdateEmployeeRolePermissionAsync(rolePermission);
            }
            else
            {
                rolePermission.EmployeeRolePermissionMaps
                    .Remove(rolePermission.EmployeeRolePermissionMaps.FirstOrDefault(x => x.EmployeeRoleId == role));

                await _permissionService.UpdateEmployeeRolePermissionAsync(rolePermission);
            }

            return true;
        }

        [HttpPost]
        public async Task<IActionResult> SavePermission(IFormCollection formCollection)
        {
            var employeeRoleId = Convert.ToInt32(formCollection["RoleId"]);

            await _permissionService.DeleteAllPermissionMapAsync(employeeRoleId);
            var permissions = await _permissionService.GetAllEmployeeRolePermissionsAsync();

            foreach (var per in permissions)
            {
                var key = $"per_{per.Id}";
                if (formCollection.ContainsKey(key))
                {
                    var rolePermission = await _permissionService.GetByIdAsync(per.Id);
                    rolePermission.EmployeeRolePermissionMaps.Add(new EmployeeRolePermissionMap
                    {
                        EmployeeRoleId = employeeRoleId,
                        PermissionId = per.Id
                    });

                    await _permissionService.UpdateEmployeeRolePermissionAsync(rolePermission);
                }
            }

            return Json(new JsonResponseModel
            {
                Status = HttpStatusCodeEnum.Success,
                Message = await _localizationService.GetResourceAsync("Message.UpdateSuccess")
            });
        }

        #endregion
    }
}