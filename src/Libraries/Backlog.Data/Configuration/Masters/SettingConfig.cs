using Backlog.Core.Domain.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.Masters
{
    public class SettingConfig : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable(nameof(Setting));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}