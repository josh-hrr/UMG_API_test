using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UMG_API.Models.DTO
{
    public class CondicionListDto
    {
        public int UMG_ID { get; set; }
        public int? UMG_Lab_ID { get; set; }
        public string UMG_Lab_Nombre { get; set; }   // null = aplica a todos los laboratorios
        public DateTime UMG_Fecha { get; set; }
        public TimeSpan UMG_Hora_Inicio { get; set; }
        public TimeSpan UMG_Hora_Fin { get; set; }
        public string UMG_Tipo { get; set; }
        public string UMG_Motivo { get; set; }
        public int UMG_Estado { get; set; }
        public DateTime UMG_Fecha_Registro { get; set; }
    }
}