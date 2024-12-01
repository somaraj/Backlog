using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;
using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;

namespace Backlog.Service.Masters
{
    public class DepartmentService : IDepartmentService
    {
        #region Fields

        protected readonly IRepository<Department> _departmentRepository;

        #endregion

        #region Ctor
        public DepartmentService(IRepository<Department> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        #endregion

        #region Methods

        public async Task<IPagedList<Department>> GetPagedListAsync(string search = "", int pageIndex = 0,
        int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "")
        {
            return await _departmentRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => !x.Deleted);
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(Department).GetProperties();
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
        public async Task<IList<Department>> GetAllAsync(bool showDeleted = false)
        {
            return await _departmentRepository.GetAllAsync(includeDeleted: showDeleted);
        }

        public async Task<IList<Department>> GetAllActiveAsync(bool showDeleted = false)
        {
            return await _departmentRepository.GetAllAsync(q => q.Where(x => x.Active), showDeleted);
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return id == 0 ? null : await _departmentRepository.GetByIdAsync(id);
        }

        public async Task<Department> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = from c in _departmentRepository.Table
                        orderby c.Id
                        where !c.Deleted && c.Name == name
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task InsertAsync(Department entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _departmentRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Department entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _departmentRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Department entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _departmentRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
