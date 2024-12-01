using Backlog.Core.Common;
using Backlog.Core.Extensions;
using Backlog.Service.Logging;
using Backlog.Service.Masters;
using Backlog.Service.Security;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Controllers.Common
{
    public class CommonController : BaseController
    {
        #region Fields

        protected readonly ISystemService _systemService;
        protected readonly IGenericAttributeService _genericAttributeService;
        protected readonly ILogService _logService;
        protected readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public CommonController(ISystemService systemService,
            IGenericAttributeService genericAttributeService,
            IWorkContext workContext,
            ILogService logService)
        {
            _systemService = systemService;
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
            _logService = logService;
        }

        #endregion

        #region Actions Related To Error

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult AccessDeniedPartial()
        {
            return PartialView();
        }

        public ActionResult NoData()
        {
            return View();
        }

        public ActionResult NoDataPartial()
        {
            return PartialView();
        }

        public IActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            Response.ContentType = "text/html";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error(int? id)
        {
            var model = new ErrorModel("ApplicationError", null);

            if (id > 0)
            {
                var log = await _logService.GetByIdAsync(id.ToInt());
                model.Message = log.FullMessage;
            }
            return View(model);
        }

        #endregion

        #region Actions

        public async Task<IActionResult> ResetCache()
        {
            await _systemService.ResetCacheAsync();

            var refererUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SetActiveProject(int id)
        {
            await HttpContext.Session.SetAsync(Constant.ActiveProjectSession, id);

            await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentEmployeeAsync(), Constant.ActiveProjectSession, id);

            var refererUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}