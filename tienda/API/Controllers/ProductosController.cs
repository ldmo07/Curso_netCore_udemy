using API.Dtos;
using API.Helpers;
using API.Helpers.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Authorize(Roles  ="Administrador")]
    public class ProductosController : BaseApiController
    {
        
        #region  FORMA SIN UNIDAD DE TRABAJO
        // private readonly IProductoRepository _productoRepository;
        // public ProductosController(IProductoRepository productoRepository)
        // {
        //     _productoRepository = productoRepository;
        // }
        #endregion

        #region  FORMA CON UNIDAD DE TRABAJO
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductosController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<ProductoListDto>>> Get([FromQuery] Params productParams){
            //var productos = await _productoRepository.GetAllAsync();
            //var productos = await _unitOfWork.Productos.GetAllAsync();
            //return _mapper.Map<List<ProductoListDto>>(productos);

            var resultado =  await _unitOfWork.Productos
                                              .GetAllAsync(productParams.PageIndex,productParams.PageSize,productParams.Search);

            var listaProductosDto =  _mapper.Map<List<ProductoListDto>>(resultado.registros);

            Response.Headers.Add("X-InlineCount",resultado.totalRegistros.ToString()); //agrega Paginado por encabezado

            return new Pager<ProductoListDto>(listaProductosDto,resultado.totalRegistros,productParams.PageIndex,productParams.PageSize,productParams.Search);
        }

        [HttpGet]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> Get11(){
            //var productos = await _productoRepository.GetAllAsync();
             var productos = await _unitOfWork.Productos.GetAllAsync();
            return _mapper.Map<List<ProductoDto>>(productos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Get(int id){
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if(producto==null)
            return NotFound(new ApiResponse(404,"El producto solicitado no existe"));

            return _mapper.Map<ProductoDto>(producto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Post(ProductoAddUpdateDto productoDto){
            var producto = _mapper.Map<Producto>(productoDto);
            _unitOfWork.Productos.Add(producto);
            await _unitOfWork.SaveAsync();
            if(producto==null){
                return BadRequest();
            }

            return CreatedAtAction(nameof(Post),new {id=producto.Id,productoDto});
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoAddUpdateDto>> Put(int id, [FromBody] ProductoAddUpdateDto productoDto){
            
            if(productoDto == null)
              return NotFound(new ApiResponse(400,"El producto solicitado no existe"));
            
            var productoBd = await _unitOfWork.Productos.GetByIdAsync(id);
            if(productoBd == null)
              return NotFound(new ApiResponse(400,"El producto solicitado no existe en bd"));
            
            var producto = _mapper.Map<Producto>(productoDto);

            _unitOfWork.Productos.Update(producto);
            await _unitOfWork.SaveAsync();

            return productoDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Producto>> Delete(int id){
            
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if(producto == null)
              return NotFound(new ApiResponse(400,"El producto solicitado no existe"));
            

            _unitOfWork.Productos.Remove(producto);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

    }
}