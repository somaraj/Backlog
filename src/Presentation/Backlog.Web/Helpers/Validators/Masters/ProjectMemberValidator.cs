using Backlog.Service.Localization;
using Backlog.Service.Masters;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Employees;
using FluentValidation;

namespace Backlog.Web.Helpers.Validators.Masters
{
    public class ProjectMemberValidator : AbstractValidator<EmployeeProjectModel>
    {
        public ProjectMemberValidator(ILocalizationService localizationService, IProjectService projectService)
        {
            RuleFor(r => r.EmployeeId)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("ProjectMemberModel.Employee.RequiredMsg"))
                .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("ProjectMemberModel.Employee.RequiredMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await projectService.GetMemberByIdAndProjectAsync(x.EmployeeId, x.ProjectId);
                        return (editedEntity != null) && (editedEntity.EmployeeId == x.EmployeeId);
                    }
                    var entity = await projectService.GetMemberByIdAndProjectAsync(x.EmployeeId, x.ProjectId);
                    return entity == null;
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("ProjectMemberModel.Employee.UniqueMsg"));
        }
    }
}