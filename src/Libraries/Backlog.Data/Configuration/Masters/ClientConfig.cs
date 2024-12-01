using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Masters;

namespace Backlog.Data.Configuration.Masters
{
    public class ClientConfig : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable(nameof(Client));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(250);

            builder.Property(p => p.ContactPerson)
                .HasMaxLength(100);

            builder.Property(p => p.PhoneNumber)
                .HasMaxLength(100);

            builder.Property(p => p.Email)
                .HasMaxLength(250);

            builder.Property(p => p.Website)
                .HasMaxLength(750);
        }
    }
}