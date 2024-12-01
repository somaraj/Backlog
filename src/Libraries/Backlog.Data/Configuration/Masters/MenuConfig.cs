using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Masters;

namespace Backlog.Data.Configuration.Masters
{
	public class MenuConfig : IEntityTypeConfiguration<Menu>
	{
		public void Configure(EntityTypeBuilder<Menu> builder)
		{
			builder.ToTable(nameof(Menu));

			builder.HasKey(x => x.Id);

			builder.Property(p => p.Name)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(p => p.SystemName)
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(p => p.ActionName)
				.HasMaxLength(100);

			builder.Property(p => p.ControllerName)
				.HasMaxLength(100);

			builder.Property(p => p.Icon)
				.HasMaxLength(100);

			builder.Property(p => p.Permission)
				.HasMaxLength(100)
				.IsRequired();
		}
	}
}