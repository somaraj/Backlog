using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Employees;

namespace Backlog.Data.Configuration.Employees
{
    public class EmployeeRoleConfig : IEntityTypeConfiguration<EmployeeRole>
    {
        public void Configure(EntityTypeBuilder<EmployeeRole> builder)
        {
            builder.ToTable(nameof(EmployeeRole));

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(250);

            builder.Property(p => p.SystemName)
                .HasMaxLength(50);

            builder.Property(p => p.SystemRole)
                .IsRequired();
        }
    }
}