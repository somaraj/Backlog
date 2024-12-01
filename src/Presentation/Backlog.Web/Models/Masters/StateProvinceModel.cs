using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Masters
{
    public class StateProvinceModel : BaseModel
    {
        [LocalizedDisplayName("StateProvinceModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("StateProvinceModel.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [LocalizedDisplayName("StateProvinceModel.Active")]
        public bool Active { get; set; }

        public int CountryId { get; set; }
    }
}
