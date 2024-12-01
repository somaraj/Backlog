using Backlog.Service.Employees;
using Backlog.Service.Localization;
using Backlog.Web.Models.Employees;
using FluentValidation;

namespace Backlog.Web.Helpers.Validators.Employees
{
    public class RegistrationValidator : AbstractValidator<EmployeeRegistrationModel>
    {
        public RegistrationValidator(ILocalizationService localizationService, IEmployeeService employeeService)
        {
            RuleFor(r => r.Employee).SetValidator(new EmployeeValidator(localizationService, employeeService));
            RuleFor(r => r.Address).SetValidator(new AddressValidator(localizationService));
        }
    }
}