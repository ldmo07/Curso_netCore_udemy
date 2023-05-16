using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ProductoDto
    {
        public int Id {get;set;}
         public  string Nombre { get; set; }
        public  decimal Precio { get; set; }
        public  DateTime FechaCreacion { get; set; }
        public  MarcaDto Marca {get;set;}
        public  CategoriaDto Categoria {get;set;}
    }
}