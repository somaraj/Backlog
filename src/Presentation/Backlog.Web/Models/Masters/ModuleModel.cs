using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Masters
{
    public class ModuleModel : BaseModel
    {
        [LocalizedDisplayName("ModuleModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("ModuleModel.Description")]
        public string? Description { get; set; }

        [LocalizedDisplayName("ModuleModel.Active")]
        public bool Active { get; set; }
    }
}
