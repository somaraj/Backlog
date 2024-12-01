using Microsoft.AspNetCore.Mvc.ModelBinding;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Helpers.Extensions
{
    public static class ModelStateErrors
    {
        public static List<ErrorModel> AllErrors(this ModelStateDictionary modelState)
        {
            var result = from ms in modelState
                         where ms.Value.Errors.Any()
                         let fieldKey = ms.Key
                         let errors = ms.Value.Errors
                         from error in errors
                         select new ErrorModel(fieldKey, error.ErrorMessage);

            return result.ToList();
        }
    }
}
