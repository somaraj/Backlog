using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface IStateProvinceService
    {
        Task<IPagedList<StateProvince>> GetPagedListAsync(int countryId, string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<StateProvince>> GetAllAsync();

        Task<IList<StateProvince>> GetAllActiveByCountryAsync(int countryId);

        Task<StateProvince> GetByIdAsync(int id);

        Task<StateProvince> GetByNameAsync(string name);

        Task InsertAsync(StateProvince entity);

        Task UpdateAsync(StateProvince entity);

        Task DeleteAsync(StateProvince entity);
    }
}
