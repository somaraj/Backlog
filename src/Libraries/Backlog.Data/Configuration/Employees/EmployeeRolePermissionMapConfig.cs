using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Employees;

namespace Backlog.Data.Configuration.Employees
{
    public class EmployeeRolePermissionMapConfig : IEntityTypeConfiguration<EmployeeRolePermissionMap>
    {
        public void Configure(EntityTypeBuilder<EmployeeRolePermissionMap> builder)
        {
            builder.ToTable(nameof(EmployeeRolePermissionMap));

            builder.HasKey(p => new
            {
                p.PermissionId,
                p.EmployeeRoleId
            });

            builder.HasOne(f => f.EmployeeRole)
                .WithMany(p => p.EmployeeRolePermissionMaps)
                .HasForeignKey(p => p.EmployeeRoleId)
                .HasConstraintName("FK_EmployeeRolePermissionMap_EmployeeRole")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(f => f.Permission)
                .WithMany(p => p.EmployeeRolePermissionMaps)
                .HasForeignKey(p => p.PermissionId)
                .HasConstraintName("FK_EmployeeRolePermissionMap_Permission")
                .OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(mapping => mapping.Id);
        }
    }
}