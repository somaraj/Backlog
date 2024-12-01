using Backlog.Core.Domain.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.Localization
{
    public class LocaleResourceConfig : IEntityTypeConfiguration<LocaleResource>
    {
        public void Configure(EntityTypeBuilder<LocaleResource> builder)
        {
            builder.ToTable(nameof(LocaleResource));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.ResourceKey)
               .HasMaxLength(100)
               .IsRequired();

            builder.HasOne(f => f.Language)
                .WithMany()
                .HasForeignKey(p => p.LanguageId)
                .HasConstraintName("FK_LocaleResource_Language")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);
        }
    }
}