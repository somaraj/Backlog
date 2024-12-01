using Backlog.Core.Common;
using Backlog.Service.Employees;
using Backlog.Service.Localization;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.WorkItems;
using FluentValidation;

namespace Backlog.Web.Helpers.Validators.WorkItems
{
    public class BacklogItemValidator : AbstractValidator<BacklogItemModel>
    {
        public BacklogItemValidator(IWorkContext workContext, ILocalizationService localizationService, IEmployeeService employeeService)
        {
            RuleFor(r => r.Title)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Title.RequiredMsg"))
                .MaximumLength(500).WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Title.MaxLengthMsg"));

            RuleFor(r => r.ModuleId)
                .NotNull().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Module.RequiredMsg"))
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Module.RequiredMsg"))
                .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Module.RequiredMsg"));

            RuleFor(r => r.SubModuleId)
               .NotNull().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.SubModule.RequiredMsg"))
               .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.SubModule.RequiredMsg"))
               .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.SubModule.RequiredMsg"));

            RuleFor(r => r.TaskTypeId)
                .NotNull().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.TaskType.RequiredMsg"))
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.TaskType.RequiredMsg"))
                .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.TaskType.RequiredMsg"));

            RuleFor(r => r.SeverityId)
                .NotNull().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Severity.RequiredMsg"))
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Severity.RequiredMsg"))
                .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Severity.RequiredMsg"));

            RuleFor(r => r.StatusId)
                .NotNull().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Status.RequiredMsg"))
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Status.RequiredMsg"))
                .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Status.RequiredMsg"));
        }
    }
}