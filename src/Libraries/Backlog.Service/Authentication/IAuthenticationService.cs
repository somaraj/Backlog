using Backlog.Core.Domain.Employees;

namespace Backlog.Service.Authentication
{
    public interface IAuthenticationService
    {
        Task SignInAsync(Employee entity, bool isPersistent);

        Task SignOutAsync();

        Task<Employee> GetAuthenticatedEmployeeAsync();
    }
}