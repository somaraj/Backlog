using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Settings;

public class CommonSettings : ISettings
{
    public string AllowedIpAddress { get; set; }
}
