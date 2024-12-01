using Backlog.Service.Localization;
using Backlog.Service.Masters;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Masters;
using FluentValidation;

namespace Backlog.Web.Helpers.Validators.Masters
{
    public class ModuleValidator : AbstractValidator<ModuleModel>
    {
        public ModuleValidator(ILocalizationService localizationService, IModuleService moduleService)
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("ModuleModel.Name.RequiredMsg"))
                .MaximumLength(100).WithMessageAwait(localizationService.GetResourceAsync("ModuleModel.Name.MaxLengthMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await moduleService.GetByNameAsync(x.Name);
                        return editedEntity == null || editedEntity.Id == x.Id;
                    }
                    var entity = await moduleService.GetByNameAsync(x.Name);
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("ModuleModel.Name.UniqueMsg"));

            RuleFor(r => r.Description)
                .MaximumLength(250).WithMessageAwait(localizationService.GetResourceAsync("ModuleModel.Description.MaxLengthMsg"));
        }
    }
}