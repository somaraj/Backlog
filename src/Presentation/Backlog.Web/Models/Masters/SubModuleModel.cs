using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Models.Masters
{
    public class SubModuleModel : BaseModel
    {
        public SubModuleModel()
        {
            AvailableModules = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableOwners = [new SelectListItem { Text = "Select", Value = "" }];
        }

        [LocalizedDisplayName("SubModuleModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("SubModuleModel.Description")]
        public string? Description { get; set; }

        [LocalizedDisplayName("SubModuleModel.Module")]
        public int ModuleId { get; set; }

        [LocalizedDisplayName("SubModuleModel.Owner")]
        public int? OwnerId { get; set; }

        public string ModuleName { get; set; }

        [LocalizedDisplayName("SubModuleModel.Active")]
        public bool Active { get; set; }

        public IList<SelectListItem> AvailableModules { get; set; }

        public IList<SelectListItem> AvailableOwners { get; set; }
    }
}
