using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface ISubModuleService
    {
        Task<IPagedList<SubModule>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<SubModule>> GetAllAsync(bool showDeleted = false);

        Task<IList<SubModule>> GetAllActiveAsync(bool showDeleted = false);

        Task<IList<SubModule>> GetAllActiveByModuleAsync(int moduleId);

        Task<SubModule> GetByIdAsync(int id);

        Task<SubModule> GetByNameAsync(string name);

        Task InsertAsync(SubModule entity);

        Task UpdateAsync(SubModule entity);

        Task DeleteAsync(SubModule entity);
    }
}
