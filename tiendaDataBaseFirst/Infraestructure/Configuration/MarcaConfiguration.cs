using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Configuration
{
    public class MarcaConfiguration : IEntityTypeConfiguration<Marca>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Marca> builder)
        {
            builder.ToTable("Marca");
            builder.HasKey(e => e.Id).HasName("PRIMARY");
            builder.Property(e => e.Nombre).HasMaxLength(100);
        }
    }
}