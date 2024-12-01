using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.WorkItems
{
    public class BacklogItemCommentModel : BaseModel
    {
        [LocalizedDisplayName("BacklogItemComment.Title")]
        public string Comment { get; set; }
    }
}
