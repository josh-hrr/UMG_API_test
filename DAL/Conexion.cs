using System.Data.SqlClient;
using System.Configuration;

namespace UMG_API.DAL
{
    public static class Conexion
    {
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["UMGConnection"].ConnectionString;

        /// <summary>
        /// Devuelve una nueva instancia de conexión SQL, sin abrir.
        /// El consumidor es responsable de abrirla/cerrarla (usar bloque using).
        /// </summary>
        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(connectionString);
        }
    }
}