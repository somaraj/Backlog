using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Masters
{
    public class ReporterModel : BaseModel
    {
        [LocalizedDisplayName("ReporterModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("ReporterModel.Description")]
        public string? Description { get; set; }

        [LocalizedDisplayName("ReporterModel.Active")]
        public bool Active { get; set; }
    }
}
