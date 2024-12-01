using FluentValidation;
using Backlog.Service.Employees;
using Backlog.Service.Localization;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Employees;

namespace Backlog.Web.Helpers.Validators.Employees
{
    public class EmployeeRoleValidator : AbstractValidator<EmployeeRoleModel>
    {
        public EmployeeRoleValidator(ILocalizationService localizationService,
        IEmployeeService employeeService)
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeRoleModel.Name.RequiredMsg"))
                .MaximumLength(50).WithMessageAwait(localizationService.GetResourceAsync("EmployeeRoleModel.Name.MaxLengthMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await employeeService.GetEmployeeRoleByNameAsync(x.Name);
                        return (editedEntity != null) && (editedEntity.Name == x.Name);
                    }

                    var entity = await employeeService.GetEmployeeRoleByNameAsync(x.Name);
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("EmployeeRoleModel.Name.UniqueMsg"));

            RuleFor(r => r.SystemName)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmployeeRoleModel.SystemName.RequiredMsg"))
                .MaximumLength(50).WithMessageAwait(localizationService.GetResourceAsync("EmployeeRoleModel.SystemName.MaxLengthMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await employeeService.GetEmployeeRoleBySystemNameAsync(x.Name);
                        return (editedEntity != null) && (editedEntity.SystemName == x.SystemName);
                    }
                    var entity = await employeeService.GetEmployeeRoleBySystemNameAsync(x.SystemName);
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("EmployeeRoleModel.SystemName.UniqueMsg"));

            RuleFor(r => r.Description)
                .MaximumLength(250).WithMessageAwait(localizationService.GetResourceAsync("EmployeeRoleModel.Description.MaxLengthMsg"));
        }
    }
}