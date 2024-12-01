using Backlog.Core.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.Employees
{
    public class EmployeeProjectMapConfig : IEntityTypeConfiguration<EmployeeProjectMap>
    {
        public void Configure(EntityTypeBuilder<EmployeeProjectMap> builder)
        {
            builder.ToTable(nameof(EmployeeProjectMap));

            builder.HasKey(x => x.Id);

            builder.HasOne(f => f.Employee)
                .WithMany()
                .HasForeignKey(p => p.EmployeeId)
                .HasConstraintName("FK_EmployeeProjectMap_Employee")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.Project)
                .WithMany()
                .HasForeignKey(p => p.ProjectId)
                .HasConstraintName("FK_EmployeeProjectMap_Project")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);
        }
    }
}