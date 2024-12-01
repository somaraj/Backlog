using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Masters;

namespace Backlog.Data.Configuration.Masters
{
    public class ProjectConfig : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable(nameof(Project));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(250);

            builder.HasOne(f => f.Client)
                .WithMany()
                .HasForeignKey(p => p.ClientId)
                .HasConstraintName("FK_Project_Client")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);
        }
    }
}