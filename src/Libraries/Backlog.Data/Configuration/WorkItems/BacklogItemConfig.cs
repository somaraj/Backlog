using Backlog.Core.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.WorkItems
{
    public class BacklogItemConfig : IEntityTypeConfiguration<BacklogItem>
    {
        public void Configure(EntityTypeBuilder<BacklogItem> builder)
        {
            builder.ToTable(nameof(BacklogItem));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Code)
                .IsRequired();

            builder.Property(p => p.Title)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasOne(f => f.Parent)
                .WithMany()
                .HasForeignKey(p => p.ParentId)
                .HasConstraintName("FK_BacklogItem_BacklogItem")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(f => f.TaskType)
                .WithMany()
                .HasForeignKey(p => p.TaskTypeId)
                .HasConstraintName("FK_BacklogItem_TaskType")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.Severity)
               .WithMany()
               .HasForeignKey(p => p.SeverityId)
               .HasConstraintName("FK_BacklogItem_Severity")
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired(true);

            builder.HasOne(f => f.Project)
                .WithMany()
                .HasForeignKey(p => p.ProjectId)
                .HasConstraintName("FK_BacklogItem_Project")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.Module)
                .WithMany()
                .HasForeignKey(p => p.ModuleId)
                .HasConstraintName("FK_BacklogItem_Module")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(f => f.SubModule)
                .WithMany()
                .HasForeignKey(p => p.SubModuleId)
                .HasConstraintName("FK_BacklogItem_SubModule")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(f => f.Sprint)
                .WithMany()
                .HasForeignKey(p => p.SprintId)
                .HasConstraintName("FK_BacklogItem_Sprint")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(f => f.Assignee)
                .WithMany()
                .HasForeignKey(p => p.AssigneeId)
                .HasConstraintName("FK_BacklogItem_Assignee")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(f => f.Status)
                .WithMany()
                .HasForeignKey(p => p.StatusId)
                .HasConstraintName("FK_BacklogItem_Status")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .HasConstraintName("FK_BacklogItem_Created_Employee")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.ModifiedBy)
                .WithMany()
                .HasForeignKey(p => p.ModifiedById)
                .HasConstraintName("FK_BacklogItem_Modified_Employee")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }
    }
}