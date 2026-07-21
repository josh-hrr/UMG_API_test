using System;
using System.Data;
using System.Data.SqlClient;
using UMG_API.DAL;

namespace UMG_API.Repositories
{
    public class LogRepository
    {
        public void Registrar(int? userId, string accion, string modulo, string descripcion)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();

                string query = @"INSERT INTO UMG_LOG (UMG_User_ID, UMG_Accion, UMG_Modulo, UMG_Descripcion)
                                  VALUES (@UserId, @Accion, @Modulo, @Descripcion)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = (object)userId ?? DBNull.Value;
                    cmd.Parameters.Add("@Accion", SqlDbType.VarChar, 50).Value = accion;
                    cmd.Parameters.Add("@Modulo", SqlDbType.VarChar, 50).Value = modulo;
                    cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar).Value = descripcion;

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}