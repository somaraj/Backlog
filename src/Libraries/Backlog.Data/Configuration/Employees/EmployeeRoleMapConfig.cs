using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Employees;

namespace Backlog.Data.Configuration.Employees
{
    public class EmployeeRoleMapConfig : IEntityTypeConfiguration<EmployeeRoleMap>
    {
        public void Configure(EntityTypeBuilder<EmployeeRoleMap> builder)
        {
            builder.ToTable(nameof(EmployeeRoleMap));

            builder.HasKey(p => new
            {
                p.EmployeeId,
                p.EmployeeRoleId
            });

            builder.HasOne(f => f.Employee)
                .WithMany(p => p.EmployeeEmployeeRoleMaps)
                .HasForeignKey(p => p.EmployeeId)
                .HasConstraintName("FK_EmployeeRoleMap_Employee")
                .IsRequired();

            builder.HasOne(p => p.EmployeeRole)
                .WithMany()
                .HasForeignKey(p => p.EmployeeRoleId)
                .HasConstraintName("FK_EmployeeRoleMap_EmployeeRole")
                .OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(p => p.Id);
        }
    }
}