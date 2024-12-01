using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;

namespace Backlog.Service.Masters
{
    public class StateProvinceService : IStateProvinceService
    {
        #region Fields

        protected readonly IRepository<StateProvince> _stateProvinceRepository;

        #endregion

        #region Ctor
        public StateProvinceService(IRepository<StateProvince> stateProvinceRepository)
        {
            _stateProvinceRepository = stateProvinceRepository;
        }
        #endregion

        #region Methods

        public async Task<IPagedList<StateProvince>> GetPagedListAsync(int countryId, string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
            int sortColumn = -1, string sortDirection = "")
        {
            return await _stateProvinceRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => x.CountryId == countryId);
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(StateProvince).GetProperties();
                    var curOrderBy = propertyInfo[sortColumn].Name + " " + sortDirection;
                    query = query.OrderBy(curOrderBy);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }

                if (!string.IsNullOrWhiteSpace(search))
                    query = query.Where(c => c.Name.Contains(search));

                return query;
            }, pageIndex, pageSize);
        }
        public async Task<IList<StateProvince>> GetAllAsync()
        {
            return await _stateProvinceRepository.GetAllAsync();
        }

        public async Task<IList<StateProvince>> GetAllActiveByCountryAsync(int countryId)
        {
            return await _stateProvinceRepository.GetAllAsync(q => q.Where(x => x.Active && x.CountryId == countryId));
        }

        public async Task<StateProvince> GetByIdAsync(int id)
        {
            return id == 0 ? null : await _stateProvinceRepository.GetByIdAsync(id);
        }

        public async Task<StateProvince> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = from c in _stateProvinceRepository.Table
                        orderby c.Id
                        where c.Name == name
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task InsertAsync(StateProvince entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _stateProvinceRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(StateProvince entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _stateProvinceRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(StateProvince entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _stateProvinceRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
