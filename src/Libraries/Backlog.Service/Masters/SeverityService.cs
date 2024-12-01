using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;

namespace Backlog.Service.Masters
{
    public class SeverityService : ISeverityService
    {
        #region Fields

        protected readonly IRepository<Severity> _severityRepository;

        #endregion

        #region Ctor
        public SeverityService(IRepository<Severity> departmentRepository)
        {
            _severityRepository = departmentRepository;
        }
        #endregion

        #region Methods

        public async Task<IPagedList<Severity>> GetPagedListAsync(string search = "", int pageIndex = 0,
        int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "")
        {
            return await _severityRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => !x.Deleted);
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(Severity).GetProperties();
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
        public async Task<IList<Severity>> GetAllAsync(bool showDeleted = false)
        {
            return await _severityRepository.GetAllAsync(includeDeleted: showDeleted);
        }

        public async Task<IList<Severity>> GetAllActiveAsync(bool showDeleted = false)
        {
            return await _severityRepository.GetAllAsync(q => q.Where(x => x.Active), showDeleted);
        }

        public async Task<Severity> GetByIdAsync(int id)
        {
            return id == 0 ? null : await _severityRepository.GetByIdAsync(id);
        }

        public async Task<Severity> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = from c in _severityRepository.Table
                        orderby c.Id
                        where !c.Deleted && c.Name == name
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task InsertAsync(Severity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _severityRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Severity entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _severityRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Severity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _severityRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
