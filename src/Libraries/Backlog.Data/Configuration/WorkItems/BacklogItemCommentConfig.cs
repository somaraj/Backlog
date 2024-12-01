using Backlog.Core.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.WorkItems
{
    public class BacklogItemCommentConfig : IEntityTypeConfiguration<BacklogItemComment>
    {
        public void Configure(EntityTypeBuilder<BacklogItemComment> builder)
        {
            builder.ToTable(nameof(BacklogItemComment));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Comment)
                .IsRequired();

            builder.HasOne(f => f.BacklogItem)
                .WithMany()
                .HasForeignKey(p => p.BacklogItemId)
                .HasConstraintName("FK_BacklogItemComment_BacklogItem")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .HasConstraintName("FK_BacklogItemComment_Created_Employee")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.ModifiedBy)
                .WithMany()
                .HasForeignKey(p => p.ModifiedById)
                .HasConstraintName("FK_BacklogItemComment_Modified_Employee")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }
    }
}