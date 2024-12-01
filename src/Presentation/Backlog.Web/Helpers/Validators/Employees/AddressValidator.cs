using FluentValidation;
using Backlog.Service.Localization;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Masters;

namespace Backlog.Web.Helpers.Validators.Employees
{
    public class AddressValidator : AbstractValidator<AddressModel>
    {
        public AddressValidator(ILocalizationService localizationService)
        {
            RuleFor(r => r.Address1)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("AddressModel.Address1.RequiredMsg"))
                .MaximumLength(750).WithMessageAwait(localizationService.GetResourceAsync("AddressModel.Address1.MaxLengthMsg"));

            RuleFor(r => r.CountryId)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("AddressModel.Country.RequiredMsg"))
                .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("AddressModel.Country.RequiredMsg"));

            RuleFor(r => r.StateProvinceId)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("AddressModel.StateProvince.RequiredMsg"))
                .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("AddressModel.StateProvince.RequiredMsg"));

            RuleFor(r => r.City)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("AddressModel.City.RequiredMsg"))
                .MaximumLength(100).WithMessageAwait(localizationService.GetResourceAsync("AddressModel.City.MaxLengthMsg"));

            RuleFor(r => r.ZipPostalCode)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("AddressModel.ZipPostalCode.RequiredMsg"))
                .MaximumLength(20).WithMessageAwait(localizationService.GetResourceAsync("AddressModel.ZipPostalCode.MaxLengthMsg"));
        }
    }
}