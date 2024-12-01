using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Masters;

namespace Backlog.Data.Configuration.Masters
{
    public class StateProvinceConfig : IEntityTypeConfiguration<StateProvince>
    {
        public void Configure(EntityTypeBuilder<StateProvince> builder)
        {
            builder.ToTable(nameof(StateProvince));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(750)
                .IsRequired();

            builder.HasOne(f => f.Country)
                .WithMany(f => f.StateProvinces)
                .HasForeignKey(p => p.CountryId)
                .HasConstraintName("FK_State_Country")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }
    }
}