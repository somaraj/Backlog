using Backlog.Core.Caching;
using Backlog.Core.Common;
using Backlog.Core.Domain.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Logging;
using Backlog.Data.Repository;

namespace Backlog.Service.Logging
{
    public class EmployeeActivityService : IEmployeeActivityService
    {
        #region Fields

        protected readonly IRepository<ActivityLog> _activityLogRepository;
        protected readonly IHttpHelper _httpHelper;
        protected readonly IWorkContext _workContext;
        protected readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public EmployeeActivityService(IRepository<ActivityLog> activityLogRepository,
            IHttpHelper httpHelper,
            IWorkContext workContext,
            ICacheManager cacheManager)
        {
            _activityLogRepository = activityLogRepository;
            _httpHelper = httpHelper;
            _workContext = workContext;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        public async Task<ActivityLog> InsertAsync(string systemName, string comment, BaseEntity entity = null)
        {
            return await InsertAsync(await _workContext.GetCurrentEmployeeAsync(), systemName, comment, entity);
        }

        public async Task<ActivityLog> InsertAsync(Employee employee, string systemName, string comment, BaseEntity entity = null)
        {
            if (employee == null)
                return null;

            var logItem = new ActivityLog
            {
                SystemName = systemName,
                EntityId = entity?.Id,
                EntityName = entity?.GetType().Name,
                EmployeeId = employee.Id,
                Comment = CommonHelper.EnsureMaximumLength(comment ?? string.Empty, 4000),
                CreatedOnUtc = DateTime.Now,
                IpAddress = _httpHelper.GetCurrentIpAddress()
            };
            await _activityLogRepository.InsertAsync(logItem);

            return logItem;
        }

        public async Task DeleteAsync(ActivityLog activityLog)
        {
            await _activityLogRepository.DeleteAsync(activityLog);
        }

        public async Task<IPagedList<ActivityLog>> GetAllAsync(DateTime? createdOnFrom = null, DateTime? createdOnTo = null,
            int? employeeId = null, int? activityLogTypeId = null, string ipAddress = null, string entityName = null, int? entityId = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return await _activityLogRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrEmpty(ipAddress))
                    query = query.Where(logItem => logItem.IpAddress.Contains(ipAddress));

                if (createdOnFrom.HasValue)
                    query = query.Where(logItem => createdOnFrom.Value <= logItem.CreatedOnUtc);
                if (createdOnTo.HasValue)
                    query = query.Where(logItem => createdOnTo.Value >= logItem.CreatedOnUtc);

                if (employeeId.HasValue && employeeId.Value > 0)
                    query = query.Where(logItem => employeeId.Value == logItem.EmployeeId);

                if (!string.IsNullOrEmpty(entityName))
                    query = query.Where(logItem => logItem.EntityName.Equals(entityName));
                if (entityId.HasValue && entityId.Value > 0)
                    query = query.Where(logItem => entityId.Value == logItem.EntityId);

                query = query.OrderByDescending(logItem => logItem.CreatedOnUtc).ThenBy(logItem => logItem.Id);

                return query;
            }, pageIndex, pageSize);
        }

        public async Task<ActivityLog> GetByIdAsync(int activityLogId)
        {
            return await _activityLogRepository.GetByIdAsync(activityLogId);
        }

        public async Task ClearAllAsync()
        {
            await _activityLogRepository.TruncateAsync();
        }

        #endregion
    }
}