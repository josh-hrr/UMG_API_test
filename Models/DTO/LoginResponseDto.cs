using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UMG_API.Models.DTO
{
    public class LoginResponseDto
    {
        public int UMG_ID { get; set; }
        public string UMG_Usuario { get; set; }
        public string UMG_Nombre { get; set; }
        public string UMG_Apellido { get; set; }
        public int UMG_Rol_ID { get; set; }
        public string UMG_Rol_Nombre { get; set; }
        public bool RequiereCambioContrasena { get; set; }
    }
}