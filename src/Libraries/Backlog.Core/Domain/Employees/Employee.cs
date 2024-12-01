using Backlog.Core.Domain.Common;
using Backlog.Core.Domain.Localization;
using Backlog.Core.Domain.Masters;

namespace Backlog.Core.Domain.Employees
{
    public class Employee : BaseEntity, ISoftDeletedEntity
    {
        private ICollection<EmployeeRoleMap> _employeeEmployeeRoleMaps;
        private ICollection<EmployeeRole> _employeeRoles;

        public Employee()
        {
            Code = Guid.NewGuid();
        }

        public Guid Code { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        public int GenderId { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public DateOnly DateOfJoin { get; set; }

        public int DepartmentId { get; set; }

        public int DesignationId { get; set; }

        public int LanguageId { get; set; }

        public int AddressId { get; set; }

        public bool SystemAccount { get; set; }

        public int FailedLoginAttempts { get; set; }

        public string? LastIPAddress { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public DateTime LastActivityDate { get; set; }

        public bool Locked { get; set; }

        public int Status { get; set; }

        public bool Deleted { get; set; }

        public string Name => $"{FirstName} {LastName}";

        public bool IsAdmin { get; set; }

        #region Roles

        public virtual ICollection<EmployeeRole> EmployeeRoles =>
            _employeeRoles ??= EmployeeEmployeeRoleMaps.Select(m => m.EmployeeRole).ToList();

        public virtual ICollection<EmployeeRoleMap> EmployeeEmployeeRoleMaps
        {
            get => _employeeEmployeeRoleMaps ??= new List<EmployeeRoleMap>();
            protected set => _employeeEmployeeRoleMaps = value;
        }

        public void AddToRole(EmployeeRoleMap role)
        {
            EmployeeEmployeeRoleMaps.Add(role);
            _employeeRoles = null;
        }

        public void RemoveFromRole(EmployeeRoleMap role)
        {
            EmployeeEmployeeRoleMaps.Remove(role);
            _employeeRoles = null;
        }

        #endregion

        public virtual Department Department { get; set; }

        public virtual Designation Designation { get; set; }

        public virtual Language Language { get; set; }

        public virtual Address Address { get; set; }
    }
}