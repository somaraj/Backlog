using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Masters
{
    public class Setting : BaseEntity
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}