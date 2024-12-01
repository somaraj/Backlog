using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;
using Backlog.Core.Caching;
using Backlog.Core.Common;
using Backlog.Core.Domain.Localization;
using Backlog.Data.Repository;
using Backlog.Service.Common;

namespace Backlog.Service.Localization
{
    public class LanguageService : ILanguageService
    {
        #region Fields

        protected readonly IRepository<Language> _languageRepository;
        protected readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor
        public LanguageService(IRepository<Language> languageRepository,
            ICacheManager cacheManager)
        {
            _languageRepository = languageRepository;
            _cacheManager = cacheManager;
        }
        #endregion

        #region Methods

        public async Task<IPagedList<Language>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
            int sortColumn = -1, string sortDirection = "")
        {
            return await _languageRepository.GetAllPagedAsync(query =>
            {
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(Language).GetProperties();
                    var curOrderBy = propertyInfo[sortColumn].Name + " " + sortDirection;
                    query = query.OrderBy(curOrderBy);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }

                if (!string.IsNullOrWhiteSpace(search))
                    query = query.Where(
                            c => c.Name.Contains(search) ||
                            c.DisplayName.Contains(search));

                return query;
            }, pageIndex, pageSize);
        }
        public async Task<IList<Language>> GetAllAsync()
        {
            var key = ServiceConstant.MenuCacheKey;

            var query = from lang in _languageRepository.Table.AsNoTracking()
                        select lang;

            return await _cacheManager.GetAsync(key, async () => await query.ToListAsync());
        }

        public async Task<IList<Language>> GetAllActiveAsync()
        {
            var key = ServiceConstant.MenuCacheKey;

            var query = from lang in _languageRepository.Table.AsNoTracking()
                        where lang.Active
                        select lang;

            return await _cacheManager.GetAsync(key, async () => await query.ToListAsync());
        }

        public async Task<Language> GetByIdAsync(int id)
        {
            return id == 0 ? null : await _languageRepository.GetByIdAsync(id);
        }

        public async Task<Language> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = from c in _languageRepository.Table
                        orderby c.Id
                        where c.Name == name
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Language> GetByCultureAsync(string culture)
        {
            if (string.IsNullOrWhiteSpace(culture))
                return null;

            var query = from c in _languageRepository.Table
                        orderby c.Id
                        where c.LanguageCulture == culture
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task InsertAsync(Language entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _languageRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Language entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _languageRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Language entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _languageRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
