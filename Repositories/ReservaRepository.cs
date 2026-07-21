using System;
using System.Data;
using System.Data.SqlClient;
using UMG_API.DAL;
using UMG_API.Models;

namespace UMG_API.Repositories
{
    public class ReservaRepository
    {
        public Reserva Crear(int userId, int labId, DateTime fecha, TimeSpan horaInicio,
                              TimeSpan horaFin, string motivo)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();

                string query = @"INSERT INTO UMG_RESERV 
                                    (UMG_User_ID, UMG_Lab_ID, UMG_Fecha_Reserva, UMG_Hora_Inicio, UMG_Hora_Fin, UMG_Motivo)
                                  OUTPUT INSERTED.UMG_ID, INSERTED.UMG_User_ID, INSERTED.UMG_Lab_ID,
                                         INSERTED.UMG_Fecha_Reserva, INSERTED.UMG_Hora_Inicio, INSERTED.UMG_Hora_Fin,
                                         INSERTED.UMG_Motivo, INSERTED.UMG_Estado, INSERTED.UMG_Fecha_Registro
                                  VALUES (@UserId, @LabId, @Fecha, @HoraInicio, @HoraFin, @Motivo)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@LabId", SqlDbType.Int).Value = labId;
                    cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = fecha.Date;
                    cmd.Parameters.Add("@HoraInicio", SqlDbType.Time).Value = horaInicio;
                    cmd.Parameters.Add("@HoraFin", SqlDbType.Time).Value = horaFin;
                    cmd.Parameters.Add("@Motivo", SqlDbType.VarChar, 150).Value = motivo;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Reserva
                            {
                                UMG_ID = Convert.ToInt32(reader["UMG_ID"]),
                                UMG_User_ID = Convert.ToInt32(reader["UMG_User_ID"]),
                                UMG_Lab_ID = Convert.ToInt32(reader["UMG_Lab_ID"]),
                                UMG_Fecha_Reserva = Convert.ToDateTime(reader["UMG_Fecha_Reserva"]),
                                UMG_Hora_Inicio = (TimeSpan)reader["UMG_Hora_Inicio"],
                                UMG_Hora_Fin = (TimeSpan)reader["UMG_Hora_Fin"],
                                UMG_Motivo = reader["UMG_Motivo"].ToString(),
                                UMG_Estado = reader["UMG_Estado"].ToString(),
                                UMG_Fecha_Registro = Convert.ToDateTime(reader["UMG_Fecha_Registro"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        // RN-01: no debe existir otra reserva ACTIVA para el mismo lab/fecha con traslape de horario
        public bool ExisteTraslape(int labId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();

                string query = @"SELECT COUNT(1) FROM UMG_RESERV
                                  WHERE UMG_Lab_ID = @LabId
                                    AND UMG_Fecha_Reserva = @Fecha
                                    AND UMG_Estado = 'R'
                                    AND UMG_Hora_Inicio < @HoraFin
                                    AND UMG_Hora_Fin > @HoraInicio";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@LabId", SqlDbType.Int).Value = labId;
                    cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = fecha.Date;
                    cmd.Parameters.Add("@HoraInicio", SqlDbType.Time).Value = horaInicio;
                    cmd.Parameters.Add("@HoraFin", SqlDbType.Time).Value = horaFin;

                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        // Verifica si existe un bloqueo (UMG_CONDI) que traslape con el horario solicitado
        public bool ExisteBloqueo(int labId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();

                string query = @"SELECT COUNT(1) FROM UMG_CONDI
                                  WHERE (UMG_Lab_ID = @LabId OR UMG_Lab_ID IS NULL)
                                    AND UMG_Fecha = @Fecha
                                    AND UMG_Estado = 1
                                    AND UMG_Hora_Inicio < @HoraFin
                                    AND UMG_Hora_Fin > @HoraInicio";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@LabId", SqlDbType.Int).Value = labId;
                    cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = fecha.Date;
                    cmd.Parameters.Add("@HoraInicio", SqlDbType.Time).Value = horaInicio;
                    cmd.Parameters.Add("@HoraFin", SqlDbType.Time).Value = horaFin;

                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public bool ExisteUsuario(int userId)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = "SELECT COUNT(1) FROM UMG_USERS WHERE UMG_ID = @UserId AND UMG_Estado = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
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