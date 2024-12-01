using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Localization
{
    public class LocaleResourceModel : BaseModel
    {
        public int LanguageId { get; set; }

        [LocalizedDisplayName("LocaleResourceModel.ResourceKey")]
        public string ResourceKey { get; set; }

        [LocalizedDisplayName("LocaleResourceModel.ResourceValue")]
        public string ResourceValue { get; set; }
    }
}
