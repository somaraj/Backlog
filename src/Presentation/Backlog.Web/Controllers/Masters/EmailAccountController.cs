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
    public class EmailAccountController : BaseController
    {
        #region Fields

        private readonly IEmailAccountService _emailAccountService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        private readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor

        public EmailAccountController(IEmailAccountService emailAccountService,
            IEmailTemplateService emailTemplateService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
            _emailAccountService = emailAccountService;
            _emailTemplateService = emailTemplateService;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _employeeActivityService = employeeActivityService;
            _workContext = workContext;
            _mapper = mapper;
        }

        #endregion

        #region Action

        public async Task<IActionResult> Index()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailAccount))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailAccount))
                return AccessDeniedPartial();

            var model = new EmailAccountModel();

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmailAccountModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailAccount))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<EmailAccount>(model);
                var employee = await _workContext.GetCurrentEmployeeAsync();

                await _emailAccountService.InsertAsync(entity);

                await _employeeActivityService.InsertAsync("EmailAccount", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), entity.Name), entity);

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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailAccount))
                return AccessDeniedPartial();

            var entity = await _emailAccountService.GetByIdAsync(id);
            if (entity == null)
                return RedirectToAction("Index");

            var model = _mapper.Map<EmailAccountModel>(entity);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmailAccountModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailAccount))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = await _emailAccountService.GetByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);

                await _emailAccountService.UpdateAsync(entity);

                await _employeeActivityService.InsertAsync("EmailAccount", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);

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

        public async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailAccount))
                return AccessDeniedPartial();

            var entity = await _emailAccountService.GetByIdAsync(id);
            if (entity == null)
                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.NoData,
                    Message = await _localizationService.GetResourceAsync("FormNoData.Description")
                });

            var emailTemplates = await _emailTemplateService.GetByEmailAccountIdAsync(id);
            var model = new EmailAccountModel
            {
                Id = id,
                EmailTemplates = _mapper.Map<List<EmailTemplateModel>>(emailTemplates)
            };

            return PartialView(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailAccount))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = await _emailAccountService.GetByIdAsync(id);

                await _emailAccountService.DeleteAsync(entity);

                await _employeeActivityService.InsertAsync("EmailAccount", string.Format(await _localizationService.GetResourceAsync("Log.RecordDeleted"), entity.Name), entity);

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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailAccount))
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

            var data = await _emailAccountService.GetPagedListAsync(search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => _mapper.Map<EmailAccountModel>(x)),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion
    }
}