using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Masters
{
    public class CountryModel : BaseModel
    {
        [LocalizedDisplayName("CountryModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("CountryModel.TwoLetterIsoCode")]
        public string TwoLetterIsoCode { get; set; }

        [LocalizedDisplayName("CountryModel.ThreeLetterIsoCode")]
        public string ThreeLetterIsoCode { get; set; }

        [LocalizedDisplayName("CountryModel.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [LocalizedDisplayName("CountryModel.Active")]
        public bool Active { get; set; }
    }
}
