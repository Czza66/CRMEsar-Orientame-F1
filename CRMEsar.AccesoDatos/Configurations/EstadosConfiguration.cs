using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRMEsar.Models;

public class EstadosConfiguration : IEntityTypeConfiguration<Estados>
{
    public void Configure(EntityTypeBuilder<Estados> builder)
    {
        builder.HasKey(e => e.EstadoId);

        builder.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
        builder.Property(e => e.NormalizedName).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(150);

        builder.HasOne(e => e.Entidad)
           .WithMany(e => e.Estados)
           .HasForeignKey(e => e.EntidadId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}