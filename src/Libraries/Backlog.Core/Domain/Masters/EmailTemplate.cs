using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Masters
{
    public class EmailTemplate : BaseEntity
    {
        public string Name { get; set; }

        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }

        public int EmailAccountId { get; set; }

        public bool Active { get; set; }

        public virtual EmailAccount EmailAccount { get; set; }
    }
}