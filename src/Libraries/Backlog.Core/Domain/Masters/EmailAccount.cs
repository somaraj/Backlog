using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Masters
{
    public class EmailAccount : BaseEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public string UserName { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Password { get; set; }

        public bool EnableSsl { get; set; }

        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public bool Active { get; set; }
    }
}