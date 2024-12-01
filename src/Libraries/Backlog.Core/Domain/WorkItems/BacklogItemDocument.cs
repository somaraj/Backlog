using Backlog.Core.Domain.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Masters;

namespace Backlog.Core.Domain.WorkItems
{
    public class BacklogItemDocument : BaseEntity
    {
        public int BacklogItemId { get; set; }

        public int DocumentId { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual BacklogItem BacklogItem { get; set; }

        public virtual Document Document { get; set; }

        public virtual Employee CreatedBy { get; set; }
    }
}
