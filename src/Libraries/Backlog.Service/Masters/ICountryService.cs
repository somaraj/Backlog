using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface ICountryService
    {
        Task<IPagedList<Country>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<Country>> GetAllAsync();

        Task<IList<Country>> GetAllActiveAsync();

        Task<Country> GetByIdAsync(int id);

        Task<Country> GetByNameAsync(string name);

        Task<Country> GetByTwoLetterIsoCodeAsync(string code);

        Task<Country> GetByThreeLetterIsoCodeAsync(string code);

        Task InsertAsync(Country entity);

        Task UpdateAsync(Country entity);

        Task DeleteAsync(Country entity);
    }
}
