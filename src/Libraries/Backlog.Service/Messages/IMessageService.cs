using Backlog.Core.Domain.Employees;

namespace Backlog.Service.Messages
{
    public interface IMessageService
    {
        Task EmailPasswordResetLinkAsync(Employee employee, string token);

        Task EmailActivationKitAsync(Employee employee);

        Task EmailWelcomeKitAsync(Employee employee);

        Task EmailRegistrationKitAsync(Employee employee);

        Task EmailNotificationAsync(Employee employee, string content);
    }
}
