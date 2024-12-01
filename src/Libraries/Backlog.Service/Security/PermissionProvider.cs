using Backlog.Core.Domain.Employees;

namespace Backlog.Service.Security
{
    public class PermissionProvider : IPermissionProvider
    {
        public static readonly EmployeeRolePermission ManageDashboard = new() { Name = "Manage Dashboard", SystemName = "ManageDashboard", RoleGroup = "Dashboard", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageCompany = new() { Name = "Manage Company", SystemName = "ManageCompany", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageMaster = new() { Name = "Manage Master", SystemName = "ManageMaster", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageEmployee = new() { Name = "Manage Employee", SystemName = "ManageEmployee", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageEmployeeRole = new() { Name = "Manage Employee Role", SystemName = "ManageEmployeeRole", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageDepartment = new() { Name = "Manage Department", SystemName = "ManageDepartment", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageReporter = new() { Name = "Manage Reporter", SystemName = "ManageReporter", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageDesignation = new() { Name = "Manage Designation", SystemName = "ManageDesignation", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageCountry = new() { Name = "Manage Country", SystemName = "ManageCountry", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageStateProvince = new() { Name = "Manage State Province", SystemName = "ManageStateProvince", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageCurrency = new() { Name = "Manage Currency", SystemName = "ManageCurrency", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageLanguage = new() { Name = "Manage Language", SystemName = "ManageLanguage", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageLocaleResource = new() { Name = "Manage Locale Resource", SystemName = "ManageLocaleResource", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageEmailAccount = new() { Name = "Manage Email Account", SystemName = "ManageEmailAccount", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageEmailTemplate = new() { Name = "Manage Email Template", SystemName = "ManageEmailTemplate", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageSetting = new() { Name = "Manage Setting", SystemName = "ManageSetting", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageSeverity = new() { Name = "Manage Severity", SystemName = "ManageSeverity", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageStatus = new() { Name = "Manage Status", SystemName = "ManageStatus", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageTaskType = new() { Name = "Manage Task Type", SystemName = "ManageTaskType", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageModule = new() { Name = "Manage Module", SystemName = "ManageModule", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageSubModule = new() { Name = "Manage Sub Module Source", SystemName = "ManageSubModule", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageClient = new() { Name = "Manage Client", SystemName = "ManageClient", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageProject = new() { Name = "Manage Project", SystemName = "ManageProject", RoleGroup = "Master", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageSprints = new() { Name = "Manage Sprints", SystemName = "ManageSprints", RoleGroup = "WorkItems", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageBoard = new() { Name = "Manage Boards", SystemName = "ManageBoard", RoleGroup = "WorkItems", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageBacklog = new() { Name = "Manage Backlog", SystemName = "ManageBacklog", RoleGroup = "WorkItems", SystemPermission = true };

        public virtual IEnumerable<EmployeeRolePermission> GetPermissions()
        {
            return
            [
                ManageDashboard,
                ManageMaster,
                ManageEmployee,
                ManageEmployeeRole,
                ManageDepartment,
                ManageDesignation,
                ManageCountry,
                ManageStateProvince,
                ManageCurrency,
                ManageLanguage,
                ManageLocaleResource,
                ManageSetting
            ];
        }
    }
}