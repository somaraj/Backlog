using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;

namespace Backlog.Service.Masters
{
    public class ProjectService : IProjectService
    {
        #region Fields

        protected readonly IRepository<Project> _projectRepository;
        protected readonly IRepository<EmployeeProjectMap> _projectMemberMapRepository;

        #endregion

        #region Ctor
        public ProjectService(IRepository<Project> projectRepository,
            IRepository<EmployeeProjectMap> projectMemberMapRepository)
        {
            _projectRepository = projectRepository;
            _projectMemberMapRepository = projectMemberMapRepository;
        }
        #endregion

        #region Methods

        public async Task<IPagedList<Project>> GetPagedListAsync(string search = "", int pageIndex = 0,
        int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "")
        {
            return await _projectRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => !x.Deleted);
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(Project).GetProperties();
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

        public async Task<IList<Project>> GetAllAsync(bool showDeleted = false)
        {
            return await _projectRepository.GetAllAsync(includeDeleted: showDeleted);
        }

        public async Task<IList<Project>> GetAllActiveAsync(bool showDeleted = false)
        {
            return await _projectRepository.GetAllAsync(q => q.Where(x => x.Active), showDeleted);
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            return id == 0 ? null : await _projectRepository.GetByIdAsync(id);
        }

        public async Task<Project> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = from c in _projectRepository.Table
                        orderby c.Id
                        where !c.Deleted && c.Name == name
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task InsertAsync(Project entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _projectRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Project entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _projectRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Project entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _projectRepository.DeleteAsync(entity);
        }

        #endregion

        #region Members Mapping

        public async Task<IPagedList<EmployeeProjectMap>> GetPagedListMembersAsync(int projectId, string search = "", int pageIndex = 0,
            int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "")
        {
            return await _projectMemberMapRepository.GetAllPagedAsync(query =>
            {
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(EmployeeProjectMap).GetProperties();
                    var curOrderBy = propertyInfo[sortColumn].Name + " " + sortDirection;
                    query = query.OrderBy(curOrderBy);
                }
                else
                {
                    query = query.OrderBy(x => x.Employee.Name);
                }

                if (!string.IsNullOrWhiteSpace(search))
                    query = query.Where(c => c.Employee.Name.Contains(search) ||
                    c.Project.Name.Contains(search));

                return query;
            }, pageIndex, pageSize);
        }

        public async Task<EmployeeProjectMap> GetMemberByIdAsync(int id)
        {
            return id == 0 ? null : await _projectMemberMapRepository.GetByIdAsync(id);
        }

        public async Task<EmployeeProjectMap> GetMemberByIdAndProjectAsync(int employeeId, int projectId)
        {
            var query = _projectMemberMapRepository.Table.AsNoTracking().Where(x => x.EmployeeId == employeeId && x.ProjectId == projectId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task InsertMemberAsync(EmployeeProjectMap entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _projectMemberMapRepository.InsertAsync(entity);
        }

        public async Task UpdateMemberAsync(EmployeeProjectMap entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _projectMemberMapRepository.UpdateAsync(entity);
        }

        public async Task DeleteMemberAsync(EmployeeProjectMap entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _projectMemberMapRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
