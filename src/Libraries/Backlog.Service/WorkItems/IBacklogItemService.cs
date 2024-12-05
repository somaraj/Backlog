using Backlog.Core.Common;
using Backlog.Core.Domain.WorkItems;

namespace Backlog.Service.WorkItems
{
    public interface IBacklogItemService
    {
        Task<IPagedList<BacklogItem>> GetPagedListAsync(int projectId, string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<BacklogItem>> GetAllAsync(int projectId);

        Task<BacklogItem> GetByIdAsync(int id);

        Task InsertAsync(BacklogItem entity, List<int> documentIds);

        Task UpdateAsync(BacklogItem entity);

        Task UpdateAsync(int id, string property, string value);


        Task DeleteAsync(BacklogItem entity);
    }
}
