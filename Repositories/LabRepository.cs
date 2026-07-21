using System;
using System.Data;
using System.Data.SqlClient;
using UMG_API.DAL;
using UMG_API.Models;

namespace UMG_API.Repositories
{
    public class LabRepository
    {
        public Lab Crear(string nombre)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();

                string query = @"INSERT INTO UMG_LABS (UMG_Nombre) 
                                  OUTPUT INSERTED.UMG_ID, INSERTED.UMG_Nombre, 
                                         INSERTED.UMG_Estado, INSERTED.UMG_Reserva, 
                                         INSERTED.UMG_Fecha_Registro
                                  VALUES (@Nombre)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 30).Value = nombre;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Lab
                            {
                                UMG_ID = Convert.ToInt32(reader["UMG_ID"]),
                                UMG_Nombre = reader["UMG_Nombre"].ToString(),
                                UMG_Estado = Convert.ToInt32(reader["UMG_Estado"]),
                                UMG_Reserva = reader["UMG_Reserva"].ToString(),
                                UMG_Fecha_Registro = Convert.ToDateTime(reader["UMG_Fecha_Registro"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public bool ExisteNombre(string nombre)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();

                string query = "SELECT COUNT(1) FROM UMG_LABS WHERE UMG_Nombre = @Nombre";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 30).Value = nombre;
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}