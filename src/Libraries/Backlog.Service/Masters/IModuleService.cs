using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface IModuleService
    {
        Task<IPagedList<Module>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<Module>> GetAllAsync(bool showDeleted = false);

        Task<IList<Module>> GetAllActiveAsync(bool showDeleted = false);

        Task<IList<Module>> GetAllActiveByProjectAsync(int projectId);

        Task<Module> GetByIdAsync(int id);

        Task<Module> GetByNameAsync(string name);

        Task InsertAsync(Module entity);

        Task UpdateAsync(Module entity);

        Task DeleteAsync(Module entity);
    }
}
