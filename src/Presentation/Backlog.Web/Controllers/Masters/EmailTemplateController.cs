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
    public class EmailTemplateController : BaseController
    {
        #region Fields

        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        private readonly IEmployeeActivityService _employeeActivityService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        protected readonly IWorkContext _workContext;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor

        public EmailTemplateController(IEmailTemplateService emailTemplateService,
            IEmailAccountService emailAccountService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWebHostEnvironment webHostEnvironment,
            IWorkContext workContext,
            IMapper mapper)
        {
            _emailTemplateService = emailTemplateService;
            _emailAccountService = emailAccountService;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _employeeActivityService = employeeActivityService;
            _webHostEnvironment = webHostEnvironment;
            _workContext = workContext;
            _mapper = mapper;
        }

        #endregion

        #region Action

        public async Task<IActionResult> Index()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailTemplate))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailTemplate))
                return AccessDeniedPartial();

            var entity = await _emailTemplateService.GetByIdAsync(id);
            if (entity == null)
                return RedirectToAction("Index");

            var model = _mapper.Map<EmailTemplateModel>(entity);
            await InitModel(model);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmailTemplateModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailTemplate))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = await _emailTemplateService.GetByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);

                await _emailTemplateService.UpdateAsync(entity);

                await _employeeActivityService.InsertAsync("EmailTemplate", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);

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

        public async Task<IActionResult> Reset()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailTemplate))
                return AccessDeniedPartial();

            var model = new PageModel
            {
                Valid = true
            };

            var defaultEmailAccount = await _emailAccountService.GetFirstActiveAsync();

            if (defaultEmailAccount == null)
            {
                model.Valid = false;
                model.Message = await _localizationService.GetResourceAsync("EmailTemplateModel.DefaultAccount.NotExistMsg");
            }

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Reset(IFormCollection formCollection)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailTemplate))
                return AccessDeniedPartial();

            //var user = _workContext.ActiveEmployee;
            var defaultEmailAccount = await _emailAccountService.GetFirstActiveAsync();

            if (defaultEmailAccount == null)
                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.ValidationError,
                    Message = await _localizationService.GetResourceAsync("EmailTemplateModel.DefaultAccount.NotExistMsg")
                });

            foreach (var val in Enum.GetValues(typeof(EmailTemplateTypeEnum)))
            {
                if (!formCollection.Keys.Any(x => x == val.ToString()))
                    continue;

                var defaultTemplatePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "email", $"{val.ToString().ToLower()}.html");

                if (!System.IO.File.Exists(defaultTemplatePath))
                    continue;

                string template = null;
                using (StreamReader sr = new StreamReader(defaultTemplatePath))
                {
                    template = sr.ReadToEnd();
                }

                //Continue if template is empty
                if (string.IsNullOrEmpty(template))
                    continue;

                var entity = await _emailTemplateService.GetByNameAsync(val.ToString());
                if (entity != null)
                {
                    entity.EmailBody = template;
                    await _emailTemplateService.UpdateAsync(entity);

                    await _employeeActivityService.InsertAsync("EmailTemplate", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);
                }
                else
                {
                    var templateModel = new EmailTemplateModel
                    {
                        Name = val.ToString(),
                        EmailSubject = val.ToString(),
                        EmailBody = template,
                        Active = true,
                        EmailAccountId = defaultEmailAccount.Id
                    };

                    var newEntity = _mapper.Map<EmailTemplate>(templateModel);

                    await _emailTemplateService.InsertAsync(newEntity);

                    await _employeeActivityService.InsertAsync("EmailTemplate", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), newEntity.Name), newEntity);
                }
            }

            return Json(new JsonResponseModel
            {
                Status = HttpStatusCodeEnum.Success,
                Message = await _localizationService.GetResourceAsync("Message.UpdateSuccess")
            });
        }

        #endregion

        #region Data

        [HttpPost]
        public async Task<IActionResult> DataRead(DataSourceRequest request)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmailTemplate))
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

            var data = await _emailTemplateService.GetPagedListAsync(search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => new EmailTemplateGridModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    EmailSubject = x.EmailSubject,
                    EmailAccount = x.EmailAccount.Name,
                    Active = x.Active
                }),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion

        #region Helper

        private async Task InitModel(EmailTemplateModel model)
        {
            var emailAccounts = await _emailAccountService.GetAllActiveAsync();
            model.AvailableEmailAccounts = emailAccounts.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
        }

        #endregion
    }
}