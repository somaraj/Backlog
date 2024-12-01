using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Employees;

namespace Backlog.Data.Configuration.Employees
{
    public class EmployeePasswordConfig : IEntityTypeConfiguration<EmployeePassword>
    {
        public void Configure(EntityTypeBuilder<EmployeePassword> builder)
        {
            builder.ToTable(nameof(EmployeePassword));

            builder.HasKey(p => p.Id);

            builder.HasOne(f => f.Employee)
                .WithMany()
                .HasForeignKey(p => p.EmployeeId)
                .HasConstraintName("FK_EmployeePassword_Employee")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}