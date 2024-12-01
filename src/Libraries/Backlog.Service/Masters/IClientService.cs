using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface IClientService
    {
        Task<IPagedList<Client>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<Client>> GetAllAsync(bool showDeleted = false);

        Task<IList<Client>> GetAllActiveAsync(bool showDeleted = false);

        Task<Client> GetByIdAsync(int id);

        Task<Client> GetByNameAsync(string name);

        Task InsertAsync(Client entity);

        Task UpdateAsync(Client entity);

        Task DeleteAsync(Client entity);
    }
}
