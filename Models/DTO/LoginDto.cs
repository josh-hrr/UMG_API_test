using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UMG_API.Models.DTO
{
    public class LoginDto
    {
        public string UMG_Usuario { get; set; }     // correo
        public string UMG_Contrasena { get; set; }
    }

}