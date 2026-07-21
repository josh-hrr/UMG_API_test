using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using UMG_API.DAL;
using UMG_API.Models;

namespace UMG_API.Repositories
{
    public class CondicionRepository
    {
        public Condicion Crear(int? labId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin,
                                string tipo, string motivo)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();

                string query = @"INSERT INTO UMG_CONDI 
                                    (UMG_Lab_ID, UMG_Fecha, UMG_Hora_Inicio, UMG_Hora_Fin, UMG_Tipo, UMG_Motivo)
                                  OUTPUT INSERTED.UMG_ID, INSERTED.UMG_Lab_ID, INSERTED.UMG_Fecha,
                                         INSERTED.UMG_Hora_Inicio, INSERTED.UMG_Hora_Fin, INSERTED.UMG_Tipo,
                                         INSERTED.UMG_Motivo, INSERTED.UMG_Estado, INSERTED.UMG_Fecha_Registro
                                  VALUES (@LabId, @Fecha, @HoraInicio, @HoraFin, @Tipo, @Motivo)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Si labId es null, se envía DBNull explícitamente (aplica a todos los laboratorios)
                    cmd.Parameters.Add("@LabId", SqlDbType.Int).Value = (object)labId ?? DBNull.Value;
                    cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = fecha.Date;
                    cmd.Parameters.Add("@HoraInicio", SqlDbType.Time).Value = horaInicio;
                    cmd.Parameters.Add("@HoraFin", SqlDbType.Time).Value = horaFin;
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 30).Value = tipo;
                    cmd.Parameters.Add("@Motivo", SqlDbType.VarChar, 150).Value = motivo;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Condicion
                            {
                                UMG_ID = Convert.ToInt32(reader["UMG_ID"]),
                                UMG_Lab_ID = reader["UMG_Lab_ID"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(reader["UMG_Lab_ID"]),
                                UMG_Fecha = Convert.ToDateTime(reader["UMG_Fecha"]),
                                UMG_Hora_Inicio = (TimeSpan)reader["UMG_Hora_Inicio"],
                                UMG_Hora_Fin = (TimeSpan)reader["UMG_Hora_Fin"],
                                UMG_Tipo = reader["UMG_Tipo"].ToString(),
                                UMG_Motivo = reader["UMG_Motivo"].ToString(),
                                UMG_Estado = Convert.ToInt32(reader["UMG_Estado"]),
                                UMG_Fecha_Registro = Convert.ToDateTime(reader["UMG_Fecha_Registro"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public bool ExisteLab(int labId)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = "SELECT COUNT(1) FROM UMG_LABS WHERE UMG_ID = @LabId AND UMG_Estado = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@LabId", SqlDbType.Int).Value = labId;
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }
    }


}