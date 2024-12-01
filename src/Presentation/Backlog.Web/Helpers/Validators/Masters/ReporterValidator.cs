using Backlog.Service.Localization;
using Backlog.Service.Masters;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Masters;
using FluentValidation;

namespace Backlog.Web.Helpers.Validators.Masters
{
    public class ReporterValidator : AbstractValidator<ReporterModel>
    {
        public ReporterValidator(ILocalizationService localizationService, IReporterService reporterService)
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("ReporterModel.Name.RequiredMsg"))
                .MaximumLength(100).WithMessageAwait(localizationService.GetResourceAsync("ReporterModel.Name.MaxLengthMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await reporterService.GetByNameAsync(x.Name);
                        return editedEntity == null || editedEntity.Id == x.Id;
                    }
                    var entity = await reporterService.GetByNameAsync(x.Name);
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("ReporterModel.Name.UniqueMsg"));

            RuleFor(r => r.Description)
                .MaximumLength(250).WithMessageAwait(localizationService.GetResourceAsync("ReporterModel.Description.MaxLengthMsg"));
        }
    }
}