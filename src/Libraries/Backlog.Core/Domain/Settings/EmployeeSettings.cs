using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Settings;

public class EmployeeSettings : ISettings
{
    public string Prefix { get; set; }

    public int Sequence { get; set; }
}
