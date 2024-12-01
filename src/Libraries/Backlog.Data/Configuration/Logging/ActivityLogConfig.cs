using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Logging;

namespace Backlog.Data.Configuration.Logging
{
    public class ActivityLogConfig : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder.ToTable(nameof(ActivityLog));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.SystemName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.EntityName)
                .HasMaxLength(500)
                .IsRequired();
        }
    }
}