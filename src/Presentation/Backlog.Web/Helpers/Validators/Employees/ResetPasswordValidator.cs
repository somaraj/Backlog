using Backlog.Service.Localization;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Employees;
using FluentValidation;

namespace Backlog.Web.Helpers.Validators.Employees
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordValidator(ILocalizationService localizationService)
        {
            RuleFor(r => r.Password)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("ResetPasswordModel.Password.RequiredMsg"))
                .MaximumLength(100).WithMessageAwait(localizationService.GetResourceAsync("ResetPasswordModel.Password.MaxLengthMsg"));
        }
    }
}