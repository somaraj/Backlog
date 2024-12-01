using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface IDepartmentService
    {
        Task<IPagedList<Department>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<Department>> GetAllAsync(bool showDeleted = false);

        Task<IList<Department>> GetAllActiveAsync(bool showDeleted = false);

        Task<Department> GetByIdAsync(int id);

        Task<Department> GetByNameAsync(string name);

        Task InsertAsync(Department entity);

        Task UpdateAsync(Department entity);

        Task DeleteAsync(Department entity);
    }
}
