using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRMEsar.Models;

public class ModulosConfiguration : IEntityTypeConfiguration<Modulos>
{
    public void Configure(EntityTypeBuilder<Modulos> builder)
    {
        builder.HasKey(m => m.moduloId);

        builder.Property(m => m.nombre).IsRequired();
        builder.Property(m => m.icono).IsRequired();
        builder.Property(m => m.orden).IsRequired();

        builder.HasOne(m => m.Estado)
               .WithMany(e => e.Modulos)
               .HasForeignKey(m => m.EstadoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}