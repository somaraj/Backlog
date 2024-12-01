using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;
using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;

namespace Backlog.Service.Masters
{
    public class DesignationService : IDesignationService
    {
        #region Fields

        protected readonly IRepository<Designation> _designationRepository;

        #endregion

        #region Ctor
        public DesignationService(IRepository<Designation> designationRepository)
        {
            _designationRepository = designationRepository;
        }
        #endregion

        #region Methods

        public async Task<IPagedList<Designation>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
            int sortColumn = -1, string sortDirection = "")
        {
            return await _designationRepository.GetAllPagedAsync(query =>
            {
                if (sortColumn >= 0)
                {
                    query = query.Where(x => !x.Deleted);
                    var propertyInfo = typeof(Designation).GetProperties();
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
        public async Task<IList<Designation>> GetAllAsync(bool showDeleted = false)
        {
            return await _designationRepository.GetAllAsync(includeDeleted: showDeleted);
        }

        public async Task<IList<Designation>> GetAllActiveAsync(bool showDeleted = false)
        {
            return await _designationRepository.GetAllAsync(q => q.Where(x => x.Active), showDeleted);
        }

        public async Task<Designation> GetByIdAsync(int id)
        {
            return id == 0 ? null : await _designationRepository.GetByIdAsync(id);
        }

        public async Task<Designation> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = from c in _designationRepository.Table
                        orderby c.Id
                        where !c.Deleted && c.Name == name
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task InsertAsync(Designation entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _designationRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Designation entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _designationRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Designation entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _designationRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
