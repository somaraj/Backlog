using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Localization
{
    public class LocaleResource : BaseEntity
    {
        public int LanguageId { get; set; }

        public string ResourceKey { get; set; }

        public string ResourceValue { get; set; }

        public virtual Language Language { get; set; }
    }
}
