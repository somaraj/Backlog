using Backlog.Core.Common;
using Backlog.Core.Domain.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Logging;

namespace Backlog.Service.Logging
{
    public partial interface IEmployeeActivityService
    {
        Task<ActivityLog> InsertAsync(string systemName, string comment, BaseEntity entity = null);

        Task<ActivityLog> InsertAsync(Employee employee, string systemName, string comment, BaseEntity entity = null);

        Task DeleteAsync(ActivityLog activityLog);

        Task<IPagedList<ActivityLog>> GetAllAsync(DateTime? createdOnFrom = null, DateTime? createdOnTo = null,
            int? employeeId = null, int? activityLogTypeId = null, string ipAddress = null, string entityName = null, int? entityId = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        Task<ActivityLog> GetByIdAsync(int activityLogId);

        Task ClearAllAsync();
    }
}
