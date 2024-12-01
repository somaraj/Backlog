using FluentValidation;
using Backlog.Service.Localization;
using Backlog.Service.Masters;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Masters;

namespace Backlog.Web.Helpers.Validators.Masters
{
    public class CountryValidator : AbstractValidator<CountryModel>
    {
        public CountryValidator(ILocalizationService localizationService, ICountryService countryService)
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("CountryModel.Name.RequiredMsg"))
                .MaximumLength(100).WithMessageAwait(localizationService.GetResourceAsync("CountryModel.Name.MaxLengthMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await countryService.GetByNameAsync(x.Name);
                        return editedEntity == null || editedEntity.Id == x.Id;
                    }
                    var entity = await countryService.GetByNameAsync(x.Name);
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("CountryModel.Name.UniqueMsg"));

            RuleFor(r => r.TwoLetterIsoCode)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("CountryModel.TwoLetterIsoCode.RequiredMsg"))
                .MinimumLength(2).WithMessageAwait(localizationService.GetResourceAsync("CountryModel.TwoLetterIsoCode.MinLengthMsg"))
                .MaximumLength(2).WithMessageAwait(localizationService.GetResourceAsync("CountryModel.TwoLetterIsoCode.MaxLengthMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await countryService.GetByTwoLetterIsoCodeAsync(x.TwoLetterIsoCode);
                        return editedEntity == null || editedEntity.Id == x.Id;
                    }
                    var entity = await countryService.GetByTwoLetterIsoCodeAsync(x.TwoLetterIsoCode);
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("CountryModel.TwoLetterIsoCode.UniqueMsg"));

            RuleFor(r => r.ThreeLetterIsoCode)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("CountryModel.ThreeLetterIsoCode.RequiredMsg"))
                .MinimumLength(3).WithMessageAwait(localizationService.GetResourceAsync("CountryModel.ThreeLetterIsoCode.MinLengthMsg"))
                .MaximumLength(3).WithMessageAwait(localizationService.GetResourceAsync("CountryModel.ThreeLetterIsoCode.MaxLengthMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await countryService.GetByThreeLetterIsoCodeAsync(x.ThreeLetterIsoCode);
                        return editedEntity == null || editedEntity.Id == x.Id;
                    }
                    var entity = await countryService.GetByThreeLetterIsoCodeAsync(x.ThreeLetterIsoCode);
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("CountryModel.ThreeLetterIsoCode.UniqueMsg"));
        }
    }
}