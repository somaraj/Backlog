using Backlog.Core.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.WorkItems
{
    public class BacklogItemHistoryConfig : IEntityTypeConfiguration<BacklogItemHistory>
    {
        public void Configure(EntityTypeBuilder<BacklogItemHistory> builder)
        {
            builder.ToTable(nameof(BacklogItemHistory));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Description)
                .IsRequired();

            builder.HasOne(f => f.BacklogItem)
                .WithMany()
                .HasForeignKey(p => p.BacklogItemId)
                .HasConstraintName("FK_BacklogItemHistory_BacklogItem")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(f => f.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .HasConstraintName("FK_BacklogItemHistory_Created_Employee")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);
        }
    }
}