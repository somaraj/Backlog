using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Localization
{
    public class Language : BaseEntity
    {
        public string Name { get; set; }

        public string LanguageCulture { get; set; }

        public string DisplayName { get; set; }

        public bool Rtl { get; set; }

        public int DisplayOrder { get; set; }

        public bool Active { get; set; }
    }
}
