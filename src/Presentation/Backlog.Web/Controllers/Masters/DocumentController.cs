using Backlog.Service.Masters;
using Backlog.Web.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Controllers.Masters
{
    public class DocumentController : BaseController
    {
        #region Fields

        protected readonly IDocumentService _documentService;

        #endregion

        #region Ctor

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        #endregion

        #region Actions

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null)
                return Json(new { success = false, message = "No file uploaded" });

            var document = await _documentService.InsertAsync(file);

            if (document == null)
                return Json(new { success = false, message = "Wrong file format" });

            return Json(new
            {
                success = true,
                token = document.Id
            });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Delete([FromBody] dynamic request)
        {
            var token = request.token;
            return NotFound("File not found");
        }

        #endregion
    }
}