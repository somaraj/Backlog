using System.Linq.Dynamic.Core;
using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;

namespace Backlog.Service.Masters
{
    public class ModuleService : IModuleService
    {
        #region Fields

        protected readonly IRepository<Module> _moduleRepository;

        #endregion

        #region Ctor
        public ModuleService(IRepository<Module> moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }
        #endregion

        #region Methods

        public async Task<IPagedList<Module>> GetPagedListAsync(string search = "", int pageIndex = 0,
        int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "")
        {
            return await _moduleRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => !x.Deleted);
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(Module).GetProperties();
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
        public async Task<IList<Module>> GetAllAsync(bool showDeleted = false)
        {
            return await _moduleRepository.GetAllAsync(includeDeleted: showDeleted);
        }

        public async Task<IList<Module>> GetAllActiveAsync(bool showDeleted = false)
        {
            return await _moduleRepository.GetAllAsync(q => q.Where(x => x.Active), showDeleted);
        }

        public async Task<IList<Module>> GetAllActiveByProjectAsync(int projectId)
        {
            return await _moduleRepository.GetAllAsync(q => q.Where(x => x.Active
            && (x.ProjectId == null || x.ProjectId == projectId)));
        }

        public async Task<Module> GetByIdAsync(int id)
        {
            return id == 0 ? null : await _moduleRepository.GetByIdAsync(id);
        }

        public async Task<Module> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = from c in _moduleRepository.Table
                        orderby c.Id
                        where !c.Deleted && c.Name == name
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task InsertAsync(Module entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _moduleRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Module entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _moduleRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Module entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _moduleRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
