using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Employees
{
    public class EmployeePassword : BaseEntity
    {
        public int EmployeeId { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual Employee Employee { get; set; }
    }
}