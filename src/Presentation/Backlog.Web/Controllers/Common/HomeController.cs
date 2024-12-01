using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Controllers.Common
{
    public class HomeController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}