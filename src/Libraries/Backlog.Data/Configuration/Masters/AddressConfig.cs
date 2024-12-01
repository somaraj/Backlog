using Backlog.Core.Domain.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.Masters
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable(nameof(Address));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Address1)
                .HasMaxLength(750)
                .IsRequired();

            builder.Property(p => p.Address2)
                .HasMaxLength(750);

            builder.Property(p => p.City)
                .HasMaxLength(100);

            builder.Property(p => p.ZipPostalCode)
                .HasMaxLength(20);

            builder.HasOne(f => f.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryId)
                .HasConstraintName("FK_Address_Country")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(f => f.StateProvince)
                .WithMany()
                .HasForeignKey(p => p.StateProvinceId)
                .HasConstraintName("FK_Address_State")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }
    }
}