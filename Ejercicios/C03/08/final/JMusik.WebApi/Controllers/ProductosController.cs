using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JMusik.Data;
using JMusik.Models;
using JMusik.Data.Contratos;

namespace JMusik.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private IProductosRepositorio _productosRepositorio;
        public ProductosController(IProductosRepositorio productosRepositorio)
        {
            _productosRepositorio = productosRepositorio;
        }

        //// GET: api/Productos
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            try
            {
                return await _productosRepositorio.ObtenerProductosAsync();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _productosRepositorio.ObtenerProductoAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return producto;
        }

        // POST: api/Productos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            try
            {
                var nuevoProducto = await _productosRepositorio.Agregar(producto);
                if (nuevoProducto == null)
                {
                    return BadRequest();
                }
                return CreatedAtAction(nameof(PostProducto), new { id = nuevoProducto.Id }, producto);

            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        // PUT: api/Productos/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> PutProducto(int id, [FromBody]Producto producto)
        {
            if (producto == null)
                return NotFound();

            var resultado = await _productosRepositorio.Actualizar(producto);
            if (!resultado)
                return BadRequest();

            return producto;

        }

        //
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProducto(int id, Producto producto)
        //{
        //    if (id != producto.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(producto).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductoExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        //// DELETE: api/Productos/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Producto>> DeleteProducto(int id)
        //{
        //    var producto = await _context.Productos.FindAsync(id);
        //    if (producto == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Productos.Remove(producto);
        //    await _context.SaveChangesAsync();

        //    return producto;
        //}

        //private bool ProductoExists(int id)
        //{
        //    return _context.Productos.Any(e => e.Id == id);
        //}
    }
}
