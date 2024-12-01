using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Employees;

namespace Backlog.Data.Configuration.Employees
{
    public class EmployeeRolePermissionConfig : IEntityTypeConfiguration<EmployeeRolePermission>
    {
        public void Configure(EntityTypeBuilder<EmployeeRolePermission> builder)
        {
            builder.ToTable(nameof(EmployeeRolePermission));

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.SystemName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.RoleGroup)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}