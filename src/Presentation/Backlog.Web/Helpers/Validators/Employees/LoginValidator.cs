using Backlog.Service.Localization;
using Backlog.Web.Models.Employees;
using FluentValidation;

namespace Backlog.Web.Helpers.Validators.Employees
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator(ILocalizationService localizationService)
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Username is required")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}