using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;
using Backlog.Service.Masters;
using Backlog.Web.Helpers.Extensions;

namespace Backlog.Web.Helpers.Attributes
{
    public sealed class SaveLastVisitedPageAttribute : TypeFilterAttribute
    {
        #region Ctor

        public SaveLastVisitedPageAttribute() : base(typeof(SaveLastVisitedPageFilter))
        {
        }

        #endregion

        #region Nested filter

        private class SaveLastVisitedPageFilter : IAsyncActionFilter
        {
            #region Fields

            protected readonly IGenericAttributeService _genericAttributeService;
            protected readonly IRepository<GenericAttribute> _genericAttributeRepository;
            protected readonly IHttpHelper _httpHelper;
            protected readonly IWorkContext _workContext;

            #endregion

            #region Ctor

            public SaveLastVisitedPageFilter(IGenericAttributeService genericAttributeService,
                IRepository<GenericAttribute> genericAttributeRepository, IHttpHelper httpHelper,
                IWorkContext workContext)
            {
                _genericAttributeService = genericAttributeService;
                _genericAttributeRepository = genericAttributeRepository;
                _httpHelper = httpHelper;
                _workContext = workContext;
            }

            #endregion

            #region Utilities

            private async Task SaveLastVisitedPageAsync(ActionExecutingContext context)
            {
                ArgumentNullException.ThrowIfNull(context);

                if (!context.HttpContext.Request.IsGetRequest())
                    return;

                var pageUrl = _httpHelper.GetCurrentPageUrl(true);

                if (string.IsNullOrEmpty(pageUrl))
                    return;

                var employee = await _workContext.GetCurrentEmployeeAsync();

                var previousPageAttribute = (await _genericAttributeService
                    .GetAttributesForEntityAsync(employee.Id, nameof(Employee)))
                    .FirstOrDefault(attribute => attribute.Key.Equals(Constant.LastVisitedPageAttribute, StringComparison.InvariantCultureIgnoreCase));

                if (previousPageAttribute == null)
                {
                    await _genericAttributeRepository.InsertAsync(new GenericAttribute
                    {
                        EntityId = employee.Id,
                        Key = Constant.LastVisitedPageAttribute,
                        KeyGroup = nameof(Employee),
                        Value = pageUrl,
                        CreatedOrUpdatedDate = DateTime.Now
                    });
                }
                else if (!pageUrl.Equals(previousPageAttribute.Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    previousPageAttribute.Value = pageUrl;
                    previousPageAttribute.CreatedOrUpdatedDate = DateTime.Now;

                    await _genericAttributeRepository.UpdateAsync(previousPageAttribute);
                }
            }

            #endregion

            #region Methods

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                await SaveLastVisitedPageAsync(context);
                if (context.Result == null)
                    await next();
            }

            #endregion
        }

        #endregion
    }
}