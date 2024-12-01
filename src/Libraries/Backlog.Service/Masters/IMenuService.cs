using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface IMenuService
    {
        Task<IList<Menu>> GetAllAsync();

        Task<IList<Menu>> GetAllAsync(Employee employee);
    }
}