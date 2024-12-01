using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Masters
{
    public class Department : BaseEntity, ISoftDeletedEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }
    }
}