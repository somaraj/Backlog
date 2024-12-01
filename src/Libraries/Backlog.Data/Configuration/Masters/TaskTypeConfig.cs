using Backlog.Core.Domain.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.Masters
{
    public class TaskTypeConfig : IEntityTypeConfiguration<TaskType>
    {
        public void Configure(EntityTypeBuilder<TaskType> builder)
        {
            builder.ToTable(nameof(TaskType));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(250);

            builder.Property(p => p.TextColor)
                .HasMaxLength(20);

            builder.Property(p => p.BackgroundColor)
                .HasMaxLength(20);

            builder.Property(p => p.IconClass)
                .HasMaxLength(50);
        }
    }
}