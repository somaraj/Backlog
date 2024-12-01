using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface IDesignationService
    {
        Task<IPagedList<Designation>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<Designation>> GetAllAsync(bool showDeleted = false);

        Task<IList<Designation>> GetAllActiveAsync(bool showDeleted = false);

        Task<Designation> GetByIdAsync(int id);

        Task<Designation> GetByNameAsync(string name);

        Task InsertAsync(Designation entity);

        Task UpdateAsync(Designation entity);

        Task DeleteAsync(Designation entity);
    }
}
