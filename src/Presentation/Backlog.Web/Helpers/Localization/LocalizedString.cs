using Microsoft.AspNetCore.Html;

namespace Backlog.Web.Helpers.Localization
{
    public class LocalizedString : HtmlString
    {
        public LocalizedString(string localized) : base(localized)
        {
            Text = localized;
        }

        public string Text { get; }
    }
}