using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRMEsar.Models;

public class SystemSettingsConfiguration : IEntityTypeConfiguration<SystemSetting>
{
    public void Configure(EntityTypeBuilder<SystemSetting> builder)
    {
        builder.ToTable("SystemSettings");

        builder.HasKey(x => x.SettingId);

        builder.HasIndex(x => x.Key)
               .IsUnique();

        builder.Property(x => x.Key)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.Group)
               .HasMaxLength(80);

        builder.Property(x => x.Description)
               .HasMaxLength(500);

        builder.Property(x => x.DataType)
               .IsRequired()
               .HasMaxLength(30);

        builder.Property(x => x.ValueString)
               .HasMaxLength(4000);

        builder.Property(x => x.ValueDecimal)
               .HasColumnType("decimal(18,4)");

        builder.Property(x => x.ValueDecimal)
               .HasPrecision(18, 4);

        builder.Property(x => x.ValueJson)
               .HasColumnType("nvarchar(max)");

        builder.Property(x => x.IsActive)
               .HasDefaultValue(true);

        builder.Property(x => x.UpdatedAtUtc)
               .HasDefaultValueSql("SYSUTCDATETIME()");
    }
}