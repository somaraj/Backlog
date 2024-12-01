using Backlog.Core.Domain.Common;

namespace Backlog.Web.Models.WorkItems
{
    public class Sprint : BaseEntity, ISoftDeletedEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public bool Completed { get; set; }

        public bool Deleted { get; set; }
    }
}
