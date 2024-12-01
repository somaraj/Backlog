using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface ISeverityService
    {
        Task<IPagedList<Severity>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<Severity>> GetAllAsync(bool showDeleted = false);

        Task<IList<Severity>> GetAllActiveAsync(bool showDeleted = false);

        Task<Severity> GetByIdAsync(int id);

        Task<Severity> GetByNameAsync(string name);

        Task InsertAsync(Severity entity);

        Task UpdateAsync(Severity entity);

        Task DeleteAsync(Severity entity);
    }
}
