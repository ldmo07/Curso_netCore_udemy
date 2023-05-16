using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Data
{
    public class TiendaContext:DbContext
    {
        #region  CONSTRUCTOR
        public TiendaContext(DbContextOptions options):base(options)
        {
        }
        #endregion


        #region  DbSet
        public DbSet<Producto> Productos {get;set;}
        public DbSet<Marca> Marcas {get;set;}
        public DbSet<Categoria> Categorias {get;set;}
        public DbSet<Usuario> Usuarios {get;set;}
        public DbSet<Rol> Roles {get;set;}
        #endregion

        #region  ONMODELCREATING
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            //llamo todas las clases que estan en la carpeta Configuration que implementan fluent API IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        #endregion
    }
}