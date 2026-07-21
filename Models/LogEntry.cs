using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UMG_API.Models
{
    public class LogEntry
    {
        public long UMG_ID { get; set; }
        public int? UMG_User_ID { get; set; }
        public string UMG_Accion { get; set; }
        public string UMG_Modulo { get; set; }
        public string UMG_Descripcion { get; set; }
        public DateTime UMG_Fecha_Registro { get; set; }
    }

}