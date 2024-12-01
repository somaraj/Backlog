using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Masters;

namespace Backlog.Data.Configuration.Masters
{
    public class GenericAttributeConfig : IEntityTypeConfiguration<GenericAttribute>
    {
        public void Configure(EntityTypeBuilder<GenericAttribute> builder)
        {
            builder.ToTable(nameof(GenericAttribute));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.KeyGroup)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Key)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Value)
                .IsRequired();
        }
    }
}