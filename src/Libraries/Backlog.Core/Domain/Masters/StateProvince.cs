using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Masters
{
    public class StateProvince : BaseEntity
    {
        public string Name { get; set; }

        public int CountryId { get; set; }

        public int DisplayOrder { get; set; }

        public bool Active { get; set; }

        public virtual Country Country { get; set; }
    }
}
