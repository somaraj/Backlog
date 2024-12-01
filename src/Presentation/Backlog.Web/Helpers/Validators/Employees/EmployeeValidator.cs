using Backlog.Service.Employees;
using Backlog.Service.Localization;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Employees;
using FluentValidation;

namespace Backlog.Web.Helpers.Validators.Employees
{
    public class EmployeeValidator : AbstractValidator<EmployeeModel>
    {
        public EmployeeValidator(ILocalizationService localizationService, IEmployeeService employeeService)
        {
            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.FirstName.RequiredMsg"))
                .MaximumLength(50).WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.FirstName.MaxLengthMsg"));

            RuleFor(r => r.LastName)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.LastName.RequiredMsg"))
                .MaximumLength(50).WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.LastName.MaxLengthMsg"));

            RuleFor(r => r.Email)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Email.RequiredMsg"))
                .MaximumLength(250).WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Email.MaxLengthMsg"))
                .EmailAddress().WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Email.InvalidEmailMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await employeeService.GetByEmailAsync(x.Email);
                        return (editedEntity != null) && (editedEntity.Email == x.Email);
                    }
                    var entity = await employeeService.GetByEmailAsync(x.Email);
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Email.UniqueMsg"));

            RuleFor(r => r.MobileNumber)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.MobileNumber.RequiredMsg"));

            RuleFor(r => r.GenderId)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Gender.RequiredMsg"))
                .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Gender.RequiredMsg"));

            RuleFor(r => r.DateOfBirth)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.DateOfBirth.RequiredMsg"))
                .GreaterThan(DateOnly.MinValue).WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.DateOfBirth.InvalidMsg"));

            RuleFor(r => r.DateOfJoin)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.DateOfJoin.RequiredMsg"))
                .GreaterThan(DateOnly.MinValue).WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.DateOfJoin.InvalidMsg"));

            RuleFor(r => r.DepartmentId)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Department.RequiredMsg"))
                .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Department.RequiredMsg"));

            RuleFor(r => r.DesignationId)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Designation.RequiredMsg"))
                .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Designation.RequiredMsg"));

            RuleFor(r => r.SelectedRoleIds)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Role.RequiredMsg"));

            RuleFor(r => r.Status)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Status.RequiredMsg"))
                .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("EmployeeModel.Status.RequiredMsg"));
        }
    }
}