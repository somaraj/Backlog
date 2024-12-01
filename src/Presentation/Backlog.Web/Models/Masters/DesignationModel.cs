using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Masters
{
    public class DesignationModel : BaseModel
    {
        [LocalizedDisplayName("DesignationModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("DesignationModel.Description")]
        public string? Description { get; set; }

        [LocalizedDisplayName("DesignationModel.Active")]
        public bool Active { get; set; }
    }
}
