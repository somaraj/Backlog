using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Localization;

namespace Backlog.Core.Common
{
    public interface IWorkContext
    {
        Task<Employee> GetCurrentEmployeeAsync();

        Task<Language> GetCurrentEmployeeLanguageAsync();
    }
}
