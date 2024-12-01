using Backlog.Core.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.WorkItems
{
    public class BacklogItemDocumentConfig : IEntityTypeConfiguration<BacklogItemDocument>
    {
        public void Configure(EntityTypeBuilder<BacklogItemDocument> builder)
        {
            builder.ToTable(nameof(BacklogItemDocument));

            builder.HasKey(x => x.Id);

            builder.HasOne(f => f.BacklogItem)
                .WithMany()
                .HasForeignKey(p => p.BacklogItemId)
                .HasConstraintName("FK_BacklogItemDocument_BacklogItem")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.Document)
                .WithMany()
                .HasForeignKey(p => p.DocumentId)
                .HasConstraintName("FK_BacklogItemDocument_Document")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .HasConstraintName("FK_BacklogItemDocument_Created_Employee")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);
        }
    }
}