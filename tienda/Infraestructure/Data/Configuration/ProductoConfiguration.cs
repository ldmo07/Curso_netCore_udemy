using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Data.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Producto> builder)
        {
            builder.ToTable("Producto");

            builder.Property(p=>p.Id).IsRequired();

            builder.Property(P=>P.Nombre).IsRequired().HasMaxLength(100);

            builder.Property(p=>p.Precio).HasColumnType("decimal(18,2)");

            builder.HasOne(p=>p.Marca).WithMany(p=>p.Productos).HasForeignKey(p=>p.MarcaId);

            builder.HasOne(p=>p.Categoria).WithMany(p=>p.Productos).HasForeignKey(p=>p.CategoriaId);
        }
    }
}