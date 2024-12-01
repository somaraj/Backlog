using Backlog.Core.Domain.Common;
using Backlog.Core.Domain.Employees;

namespace Backlog.Core.Domain.WorkItems
{
    public class BacklogItemComment : BaseEntity
    {
        public int BacklogItemId { get; set; }

        public string Comment { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual BacklogItem BacklogItem { get; set; }

        public virtual Employee CreatedBy { get; set; }

        public virtual Employee ModifiedBy { get; set; }
    }
}
