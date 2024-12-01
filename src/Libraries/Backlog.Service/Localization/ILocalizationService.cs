using Backlog.Core.Common;
using Backlog.Core.Domain.Localization;

namespace Backlog.Service.Localization
{
    public interface ILocalizationService
    {
        Task<IPagedList<LocaleResource>> GetPagedListAsync(int languageId, string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<LocaleResource>> GetAllAsync();

        Task<IList<LocaleResource>> GetByLanguageIdAsync(int id);

        Task<LocaleResource> GetByIdAsync(int id);

        Task<string> GetResourceAsync(int languageId, string resourceKey, bool logIfNotFound = true,
            string defaultValue = "", bool returnEmptyIfNotFound = false);

        Task<string> GetResourceAsync(string resourceKey);

        Task<LocaleResource> GetResourceByKeyAsync(int languageId,string resourceKey);

        Task<IList<LocaleResource>> GetAllMenuResourceAsync(int languageId);

        Task InsertAsync(LocaleResource entity);

        Task UpdateAsync(LocaleResource entity);

        Task DeleteAsync(LocaleResource entity);
    }
}
