using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse()
        {
            
        }
        public ApiResponse(int statusCode,string message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaulMessage(StatusCode);
        }

        private string GetDefaulMessage(int statusCode){
            return statusCode switch {
                400=>  "Has realizado una peticion incorrecta",
                401 => "Usuario no autorizado",
                404 => "El recurso que has solictado no existe",
                405 => "Este metodo http no esta permitido en el servidor",
                500 => "Error en el servidor comunicate con el Administrador",
                _ => "Error desconocido"
            };
        }
    }
}