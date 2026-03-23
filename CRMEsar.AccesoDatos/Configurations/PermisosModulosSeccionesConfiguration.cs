using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PermisosModulosSeccionesConfiguration : IEntityTypeConfiguration<PermisosModulosSecciones>
{
    public void Configure(EntityTypeBuilder<PermisosModulosSecciones> builder)
    {
        builder.HasKey(p => p.PermisoId);

        builder.Property(p => p.Temporal).IsRequired();

        builder.HasOne(p => p.applicationRole)
               .WithMany(r => r.PermisosModulosSecciones)
               .HasForeignKey(p => p.RolId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Modulo)
               .WithMany(m => m.permisosModulosSecciones) // <-- esto evita la duplicación
               .HasForeignKey(p => p.ModuloId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Seccion)
               .WithMany(s => s.permisosModulosSecciones) // <-- esto también
               .HasForeignKey(p => p.SeccionId)
               .OnDelete(DeleteBehavior.Restrict);
    }

}