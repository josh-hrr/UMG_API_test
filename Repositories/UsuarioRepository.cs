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
    public class UsuarioRepository
    {
        public Usuario Crear(string correo, string hashContrasena, string nombre, string apellido, int rolId)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();

                string query = @"INSERT INTO UMG_USERS 
                                    (UMG_Usuario, UMG_Contrasena, UMG_Nombre, UMG_Apellido, UMG_Rol_ID)
                                  OUTPUT INSERTED.UMG_ID, INSERTED.UMG_Usuario, INSERTED.UMG_Nombre, 
                                         INSERTED.UMG_Apellido, INSERTED.UMG_Rol_ID, INSERTED.UMG_Estado,
                                         INSERTED.UMG_Ingreso, INSERTED.UMG_Fecha_Creacion
                                  VALUES (@Correo, @Contrasena, @Nombre, @Apellido, @RolId)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Correo", SqlDbType.VarChar, 100).Value = correo;
                    cmd.Parameters.Add("@Contrasena", SqlDbType.VarChar, 255).Value = hashContrasena;
                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = nombre;
                    cmd.Parameters.Add("@Apellido", SqlDbType.VarChar, 50).Value = apellido;
                    cmd.Parameters.Add("@RolId", SqlDbType.Int).Value = rolId;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                UMG_ID = Convert.ToInt32(reader["UMG_ID"]),
                                UMG_Usuario = reader["UMG_Usuario"].ToString(),
                                UMG_Nombre = reader["UMG_Nombre"].ToString(),
                                UMG_Apellido = reader["UMG_Apellido"].ToString(),
                                UMG_Rol_ID = Convert.ToInt32(reader["UMG_Rol_ID"]),
                                UMG_Estado = Convert.ToInt32(reader["UMG_Estado"]),
                                UMG_Ingreso = Convert.ToInt32(reader["UMG_Ingreso"]),
                                UMG_Fecha_Creacion = Convert.ToDateTime(reader["UMG_Fecha_Creacion"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public bool ExisteCorreo(string correo)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = "SELECT COUNT(1) FROM UMG_USERS WHERE UMG_Usuario = @Correo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Correo", SqlDbType.VarChar, 100).Value = correo;
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public bool ExisteRol(int rolId)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = "SELECT COUNT(1) FROM UMG_ROLES WHERE UMG_ID = @RolId AND UMG_Estado = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@RolId", SqlDbType.Int).Value = rolId;
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }
    }
}