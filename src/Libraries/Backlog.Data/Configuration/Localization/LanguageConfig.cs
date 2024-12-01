using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Localization;

namespace Backlog.Data.Configuration.Localization
{
    public class LanguageConfig : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable(nameof(Language));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(750)
                .IsRequired();

            builder.Property(p => p.LanguageCulture)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(p => p.DisplayName)
                .HasMaxLength(750)
                .IsRequired();
        }
    }
}