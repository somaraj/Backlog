using Backlog.Core.Common;
using Backlog.Core.Domain.Localization;

namespace Backlog.Service.Localization
{
    public interface ILanguageService
    {
        Task<IPagedList<Language>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<Language>> GetAllAsync();

        Task<IList<Language>> GetAllActiveAsync();

        Task<Language> GetByIdAsync(int id);

        Task<Language> GetByNameAsync(string name);

        Task<Language> GetByCultureAsync(string culture);

        Task InsertAsync(Language entity);

        Task UpdateAsync(Language entity);

        Task DeleteAsync(Language entity);
    }
}
