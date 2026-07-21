using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UMG_API.DAL;
using UMG_API.Models;
using UMG_API.Models.DTO;

namespace UMG_API.Repositories
{
    public class LabRepository
    {
        public List<LabListDto> ObtenerTodos()
        {
            var labs = new List<LabListDto>();

            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();

                string query = @"SELECT UMG_ID, UMG_Nombre, UMG_Estado, UMG_Reserva, UMG_Fecha_Registro
                          FROM UMG_LABS
                          ORDER BY UMG_Nombre";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        labs.Add(new LabListDto
                        {
                            UMG_ID = Convert.ToInt32(reader["UMG_ID"]),
                            UMG_Nombre = reader["UMG_Nombre"].ToString(),
                            UMG_Estado = Convert.ToInt32(reader["UMG_Estado"]),
                            UMG_Reserva = reader["UMG_Reserva"].ToString(),
                            UMG_Fecha_Registro = Convert.ToDateTime(reader["UMG_Fecha_Registro"])
                        });
                    }
                }
            }

            return labs;
        }

        // Verifica que el laboratorio exista (para editar)
        public bool Existe(int id)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = "SELECT COUNT(1) FROM UMG_LABS WHERE UMG_ID = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        // Verifica nombre duplicado, EXCLUYENDO el propio registro que se está editando
        public bool ExisteNombreEnOtro(string nombre, int idExcluir)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = "SELECT COUNT(1) FROM UMG_LABS WHERE UMG_Nombre = @Nombre AND UMG_ID <> @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 30).Value = nombre;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idExcluir;
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }


        public bool Actualizar(int id, string nombre, int estado)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = @"UPDATE UMG_LABS 
                          SET UMG_Nombre = @Nombre, UMG_Estado = @Estado
                          WHERE UMG_ID = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 30).Value = nombre;
                    cmd.Parameters.Add("@Estado", SqlDbType.Int).Value = estado;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

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