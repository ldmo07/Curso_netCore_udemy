using Core.Entities;
using Core.Interfaces;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(TiendaContext context):base(context)
        {
        }

        public async Task<IEnumerable<Producto>> GetProductosMasCaros(int cantidad) =>
                await _context.Productos
                .OrderByDescending(p=>p.Precio)
                .Take(cantidad)
                .ToListAsync();

        public override async Task<Producto> GetByIdAsync(int id,bool noTracking=true){
            
            var queryProducto = noTracking ?  _context.Productos.AsNoTracking() : _context.Productos;
            
            return await queryProducto
            .Include(p=>p.Marca)
            .Include(p=>p.Categoria)
            .FirstOrDefaultAsync(p=>p.Id == id);
        }

        public override async Task<IEnumerable<Producto>> GetAllAsync(bool noTracking=true){

            var queryProducto = noTracking ?  _context.Productos.AsNoTracking() : _context.Productos;

            return await queryProducto
            .Include(u=>u.Marca)
            .Include(u=>u.Categoria)
            .ToListAsync();
        }

        public override async Task<(int totalRegistros,IEnumerable<Producto>registros)>GetAllAsync(int pageIndex,int pageSize,string search,bool noTracking=true){
            
            var queryProducto = noTracking ?  _context.Productos.AsNoTracking() : _context.Productos;


            if(!string.IsNullOrEmpty(search)){
                queryProducto = queryProducto.Where(p=>p.Nombre.ToLower().Contains(search));
            }

            var totalRegistros = await queryProducto.CountAsync();

            var registros = await queryProducto
                                           .Include(u=>u.Categoria)
                                           .Include(u=>u.Marca)
                                           .Skip((pageIndex-1)*pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

            return (totalRegistros,registros);
        }
                
    }
}