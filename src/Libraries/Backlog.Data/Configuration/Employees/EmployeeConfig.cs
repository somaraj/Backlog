using Backlog.Core.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.Employees
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(nameof(Employee));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Code)
                .IsRequired();

            builder.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.MobileNumber)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(p => p.Email)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(p => p.LastIPAddress)
                .HasMaxLength(100);

            builder.HasOne(f => f.Department)
                .WithMany()
                .HasForeignKey(p => p.DepartmentId)
                .HasConstraintName("FK_Employee_Department")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.Designation)
                .WithMany()
                .HasForeignKey(p => p.DesignationId)
                .HasConstraintName("FK_Employee_Designation")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.Language)
                .WithMany()
                .HasForeignKey(p => p.LanguageId)
                .HasConstraintName("FK_Employee_Language")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.Address)
                .WithMany()
                .HasForeignKey(p => p.AddressId)
                .HasConstraintName("FK_Employee_Address")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.Ignore(p => p.EmployeeRoles);
            builder.Ignore(p => p.IsAdmin);
        }
    }
}