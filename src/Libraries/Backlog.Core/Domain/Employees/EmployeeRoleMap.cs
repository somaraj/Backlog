using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Employees
{
    public class EmployeeRoleMap : BaseEntity
    {
        public int EmployeeId { get; set; }

        public int EmployeeRoleId { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual EmployeeRole EmployeeRole { get; set; }
    }
}