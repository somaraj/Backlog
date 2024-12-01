using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface IProjectService
    {
        Task<IPagedList<Project>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<Project>> GetAllAsync(bool showDeleted = false);

        Task<IList<Project>> GetAllActiveAsync(bool showDeleted = false);

        Task<Project> GetByIdAsync(int id);

        Task<Project> GetByNameAsync(string name);

        Task InsertAsync(Project entity);

        Task UpdateAsync(Project entity);

        Task DeleteAsync(Project entity);

        #region Members Mapping

        Task<IPagedList<EmployeeProjectMap>> GetPagedListMembersAsync(int projectId, string search = "", int pageIndex = 0,
            int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "");

        Task<EmployeeProjectMap> GetMemberByIdAsync(int id);

        Task<EmployeeProjectMap> GetMemberByIdAndProjectAsync(int employeeId, int projectId);

        Task InsertMemberAsync(EmployeeProjectMap entity);

        Task UpdateMemberAsync(EmployeeProjectMap entity);

        Task DeleteMemberAsync(EmployeeProjectMap entity);

        #endregion
    }
}
