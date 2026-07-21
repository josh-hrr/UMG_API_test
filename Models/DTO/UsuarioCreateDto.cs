using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UMG_API.Models.DTO
{
    public class UsuarioCreateDto
    {
        public string UMG_Usuario { get; set; }      // correo
        public string UMG_Contrasena { get; set; }   // texto plano, se hashea en el Service
        public string UMG_Nombre { get; set; }
        public string UMG_Apellido { get; set; }
        public int UMG_Rol_ID { get; set; }
    }

}