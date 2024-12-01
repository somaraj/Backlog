using Backlog.Core.Common;
using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Controllers.Common
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    [SaveIpAddress]
    [SaveLastActivity]
    [SaveLastVisitedPage]
    [ValidateIpAddress]
    public class BaseController : Controller
    {
        #region Actions

        protected IActionResult AccessDenied()
        {
            return RedirectToAction("AccessDenied", "Common");
        }

        protected IActionResult AccessDeniedPartial()
        {
            return RedirectToAction("AccessDeniedPartial", "Common");
        }

        protected IActionResult AccessDeniedDataRead()
        {
            return Json(new
            {
                error = "Access denied!"
            });
        }

        protected IActionResult NoData()
        {
            return RedirectToAction("NoData", "Common");
        }

        protected IActionResult NoDataPartial()
        {
            return RedirectToAction("NoDataPartial", "Common");
        }

        #endregion Actions

        #region Helpers

        public async Task<int> GetProjectIdFromSession()
        {
            return await HttpContext.Session.GetAsync<int>(Constant.ActiveProjectSession);
        }

        #endregion
    }
}