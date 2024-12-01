using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Localization
{
    public class LanguageModel : BaseModel
    {
        public LanguageModel()
        {
            DisplayOrder = 1;
        }

        [LocalizedDisplayName("LanguageModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("LanguageModel.LanguageCulture")]
        public string LanguageCulture { get; set; }

        [LocalizedDisplayName("LanguageModel.DisplayName")]
        public string DisplayName { get; set; }

        [LocalizedDisplayName("LanguageModel.Rtl")]
        public bool Rtl { get; set; }

        [LocalizedDisplayName("LanguageModel.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [LocalizedDisplayName("LanguageModel.Active")]
        public bool Active { get; set; }
    }
}
