using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Logging;

namespace Backlog.Service.Logging
{
    public partial interface ILogService
    {
        bool IsEnabled(LogLevel level);

        Task<IPagedList<Log>> GetAllAsync(DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = "", LogLevel? logLevel = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        Task<Log> GetByIdAsync(int logId);

        Task<IList<Log>> GetByIdsAsync(int[] logIds);

        Task InformationAsync(string message, Exception exception = null, Employee employee = null);

        void Information(string message, Exception exception = null, Employee employee = null);

        Task WarningAsync(string message, Exception exception = null, Employee employee = null);

        void Warning(string message, Exception exception = null, Employee employee = null);

        Task ErrorAsync(string message, Exception exception = null, Employee employee = null);

        Task<int> ErrorAndGetIdAsync(string message, Exception exception = null, Employee employee = null);

        void Error(string message, Exception exception = null, Employee employee = null);

        Task DeleteAsync(Log log);

        Task DeleteAsync(IList<Log> logs);

        Task ClearAsync(DateTime? olderThan = null);
    }
}