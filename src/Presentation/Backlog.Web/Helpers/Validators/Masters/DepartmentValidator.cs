using FluentValidation;
using Backlog.Service.Localization;
using Backlog.Service.Masters;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Masters;

namespace Backlog.Web.Helpers.Validators.Masters
{
    public class DepartmentValidator : AbstractValidator<DepartmentModel>
    {
        public DepartmentValidator(ILocalizationService localizationService, IDepartmentService departmentService)
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("DepartmentModel.Name.RequiredMsg"))
                .MaximumLength(100).WithMessageAwait(localizationService.GetResourceAsync("DepartmentModel.Name.MaxLengthMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await departmentService.GetByNameAsync(x.Name);
                        return editedEntity == null || editedEntity.Id == x.Id;
                    }
                    var entity = await departmentService.GetByNameAsync(x.Name);
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("DepartmentModel.Name.UniqueMsg"));

            RuleFor(r => r.Description)
                .MaximumLength(250).WithMessageAwait(localizationService.GetResourceAsync("DepartmentModel.Description.MaxLengthMsg"));
        }
    }
}