using FluentValidation;
using Backlog.Service.Localization;
using Backlog.Service.Masters;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Masters;

namespace Backlog.Web.Helpers.Validators.Masters
{
    public class EmailTemplateValidator : AbstractValidator<EmailTemplateModel>
    {
        public EmailTemplateValidator(ILocalizationService localizationService, IEmailTemplateService emailTemplateService)
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmailTemplateModel.Name.RequiredMsg"))
                .MaximumLength(100).WithMessageAwait(localizationService.GetResourceAsync("EmailTemplateModel.Name.MaxLengthMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await emailTemplateService.GetByNameAsync(x.Name);
                        return editedEntity == null || editedEntity.Id == x.Id;
                    }
                    var entity = await emailTemplateService.GetByNameAsync(x.Name);
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("EmailTemplateModel.Name.UniqueMsg"));

            RuleFor(r => r.EmailSubject)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmailTemplateModel.EmailSubject.RequiredMsg"))
                .MaximumLength(250).WithMessageAwait(localizationService.GetResourceAsync("EmailTemplateModel.EmailSubject.MaxLengthMsg"));

            RuleFor(r => r.EmailAccountId)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("EmailTemplateModel.EmailAccount.RequiredMsg"));
        }
    }
}