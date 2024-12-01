using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;
using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;

namespace Backlog.Service.Masters
{
    public class CountryService : ICountryService
    {
        #region Fields

        protected readonly IRepository<Country> _countryRepository;

        #endregion

        #region Ctor
        public CountryService(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }
        #endregion

        #region Methods

        public async Task<IPagedList<Country>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
            int sortColumn = -1, string sortDirection = "")
        {
            return await _countryRepository.GetAllPagedAsync(query =>
            {
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(Country).GetProperties();
                    var curOrderBy = propertyInfo[sortColumn].Name + " " + sortDirection;
                    query = query.OrderBy(curOrderBy);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }

                if (!string.IsNullOrWhiteSpace(search))
                    query = query.Where(c =>
                        c.Name.Contains(search) ||
                        c.TwoLetterIsoCode.Contains(search) ||
                        c.ThreeLetterIsoCode.Contains(search));

                return query;
            }, pageIndex, pageSize);
        }
        public async Task<IList<Country>> GetAllAsync()
        {
            return await _countryRepository.GetAllAsync();
        }

        public async Task<IList<Country>> GetAllActiveAsync()
        {
            return await _countryRepository.GetAllAsync(q => q.Where(x => x.Active));
        }

        public async Task<Country> GetByIdAsync(int id)
        {
            return id == 0 ? null : await _countryRepository.GetByIdAsync(id);
        }

        public async Task<Country> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = from c in _countryRepository.Table
                        orderby c.Id
                        where c.Name == name
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Country> GetByTwoLetterIsoCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            var query = from c in _countryRepository.Table
                        orderby c.Id
                        where c.TwoLetterIsoCode == code
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Country> GetByThreeLetterIsoCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            var query = from c in _countryRepository.Table
                        orderby c.Id
                        where c.ThreeLetterIsoCode == code
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task InsertAsync(Country entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _countryRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Country entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _countryRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Country entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _countryRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
