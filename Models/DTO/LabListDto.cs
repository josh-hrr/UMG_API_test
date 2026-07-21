using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UMG_API.Models.DTO
{
    public class LabListDto
    {
        public int UMG_ID { get; set; }
        public string UMG_Nombre { get; set; }
        public int UMG_Estado { get; set; }
        public string UMG_Reserva { get; set; }
        public DateTime UMG_Fecha_Registro { get; set; }
    }

}