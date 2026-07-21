using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UMG_API.Models.DTO
{
    public class ReservaCreateDto
    {
        public int UMG_User_ID { get; set; }
        public int UMG_Lab_ID { get; set; }
        public DateTime UMG_Fecha_Reserva { get; set; }
        public TimeSpan UMG_Hora_Inicio { get; set; }
        public TimeSpan UMG_Hora_Fin { get; set; }
        public string UMG_Motivo { get; set; }
    }

}