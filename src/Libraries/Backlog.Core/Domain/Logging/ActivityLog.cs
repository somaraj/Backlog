using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Logging
{
    public class ActivityLog : BaseEntity
    {
        public string SystemName { get; set; }

        public int? EntityId { get; set; }

        public string EntityName { get; set; }

        public int EmployeeId { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public virtual string IpAddress { get; set; }
    }
}