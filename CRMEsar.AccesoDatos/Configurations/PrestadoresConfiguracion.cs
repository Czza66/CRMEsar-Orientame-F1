using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMEsar.AccesoDatos.Configurations
{
    internal class PrestadoresConfiguracion : IEntityTypeConfiguration<Prestadores>
    {
        public void Configure(EntityTypeBuilder<Prestadores> builder)
        {
            // 📌 Nombre de la tabla
            builder.ToTable("Prestadores");

            // 🔑 PK
            builder.HasKey(p => p.PrestadorId);

            // 🔹 Propiedades
            builder.Property(p => p.Profesion)
                   .HasMaxLength(150);

            builder.Property(p => p.TipoPrestador)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Celular)
                   .HasMaxLength(70);

            builder.Property(p => p.TipoServicio)
                   .HasMaxLength(100);

            builder.Property(p => p.RecomendadoPor)
                   .HasMaxLength(150);

            // 🔗 Relación N–1 con Estados (SIN navegación inversa)
            builder.HasOne(p => p.Estado)
                   .WithMany()
                   .HasForeignKey(p => p.EstadoId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔗 Relación 1–1 con ApplicationUser
            builder.HasOne(p => p.User)
                   .WithOne(u => u.Prestador)
                   .HasForeignKey<Prestadores>(p => p.UserID)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔐 Índice ÚNICO (garantiza 1–1)
            builder.HasIndex(p => p.UserID)
                   .IsUnique();
        }
    }
}
