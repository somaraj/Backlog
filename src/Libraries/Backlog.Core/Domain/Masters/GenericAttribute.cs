using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Masters
{
    public class GenericAttribute : BaseEntity
    {
        public int EntityId { get; set; }

        public string KeyGroup { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public DateTime? CreatedOrUpdatedDate { get; set; }
    }
}
