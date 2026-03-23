using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRMEsar.Models;

public class ModulosSeccionesConfiguration : IEntityTypeConfiguration<ModulosSecciones>
{
    public void Configure(EntityTypeBuilder<ModulosSecciones> builder)
    {
        builder.HasKey(ms => ms.seccionId);

        builder.Property(ms => ms.nombre).IsRequired();
        builder.Property(ms => ms.descripcion).IsRequired();
        builder.Property(ms => ms.orden).IsRequired();

        builder.HasOne(ms => ms.Modulo)
               .WithMany(m => m.ModulosSecciones)
               .HasForeignKey(ms => ms.ModuloId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ms => ms.Estado)
               .WithMany(e => e.ModulosSecciones)
               .HasForeignKey(ms => ms.EstadoId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ms => ms.seccionPadre)
               .WithMany()
               .HasForeignKey(ms => ms.seccionPadreId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}