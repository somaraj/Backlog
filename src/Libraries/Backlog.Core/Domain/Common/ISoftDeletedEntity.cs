namespace Backlog.Core.Domain.Common
{
    public interface ISoftDeletedEntity
    {
        bool Deleted { get; set; }
    }
}
