using Backlog.Core.Domain.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Core.Domain.Employees
{
    public class EmployeeProjectMap : BaseEntity
    {
        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        public bool CanReport { get; set; }

        public bool CanClose { get; set; }

        public bool CanReOpen { get; set; }

        public bool CanComment { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Project Project { get; set; }
    }
}