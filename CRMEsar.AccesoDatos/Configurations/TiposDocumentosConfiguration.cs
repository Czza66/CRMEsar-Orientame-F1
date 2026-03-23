using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRMEsar.Models;

public class TiposDocumentosConfiguration : IEntityTypeConfiguration<TiposDocumentos>
{
    public void Configure(EntityTypeBuilder<TiposDocumentos> builder)
    {
        builder.HasKey(td => td.TiposDocumentosId);

        builder.Property(td => td.TipoDocumento)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(td => td.Abreviatura)
               .IsRequired()
               .HasMaxLength(10);

        builder.Property(td => td.IdAntiguo)
               .IsRequired();
        builder.HasOne(td => td.Pais)
               .WithMany(p => p.TiposDocumentos)
               .HasForeignKey(td => td.PaisId)
               .OnDelete(DeleteBehavior.Restrict); 

        builder.HasOne(td => td.Estado)
               .WithMany() 
               .HasForeignKey(td => td.EstadoId)
               .OnDelete(DeleteBehavior.Restrict); 

        builder.ToTable("TipoDocumentos");
    }
}