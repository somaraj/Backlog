using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface IStatusService
    {
        Task<IPagedList<Status>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<Status>> GetAllAsync(bool showDeleted = false);

        Task<IList<Status>> GetAllActiveAsync(bool showDeleted = false);

        Task<Status> GetByIdAsync(int id);

        Task<Status> GetByNameAsync(string name);

        Task InsertAsync(Status entity);

        Task UpdateAsync(Status entity);

        Task DeleteAsync(Status entity);
    }
}
