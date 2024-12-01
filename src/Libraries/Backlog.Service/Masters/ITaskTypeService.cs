using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface ITaskTypeService
    {
        Task<IPagedList<TaskType>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<TaskType>> GetAllAsync(bool showDeleted = false);

        Task<IList<TaskType>> GetAllActiveAsync(bool showDeleted = false);

        Task<TaskType> GetByIdAsync(int id);

        Task<TaskType> GetByNameAsync(string name);

        Task InsertAsync(TaskType entity);

        Task UpdateAsync(TaskType entity);

        Task DeleteAsync(TaskType entity);
    }
}
