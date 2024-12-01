using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Masters
{
    public class DepartmentModel : BaseModel
    {
        [LocalizedDisplayName("DepartmentModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("DepartmentModel.Description")]
        public string? Description { get; set; }

        [LocalizedDisplayName("DepartmentModel.Active")]
        public bool Active { get; set; }
    }
}
