using Backlog.Core.Domain.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.Masters
{
    public class SubModuleConfig : IEntityTypeConfiguration<SubModule>
    {
        public void Configure(EntityTypeBuilder<SubModule> builder)
        {
            builder.ToTable(nameof(SubModule));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(250);

            builder.HasOne(f => f.Module)
                .WithMany()
                .HasForeignKey(p => p.ModuleId)
                .HasConstraintName("FK_SubModule_Module")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .HasConstraintName("FK_SubModule_Employee")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }
    }
}