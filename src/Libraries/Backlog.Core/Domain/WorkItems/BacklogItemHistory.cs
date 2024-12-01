using Backlog.Core.Domain.Common;
using Backlog.Core.Domain.Employees;

namespace Backlog.Core.Domain.WorkItems
{
    public class BacklogItemHistory : BaseEntity
    {
        public int BacklogItemId { get; set; }

        public string Description { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual BacklogItem BacklogItem { get; set; }

        public virtual Employee CreatedBy { get; set; }
    }
}
