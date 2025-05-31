using System;
using System.Configuration;
using System.Data.SQLite;

namespace ProyectoMetodos.Models
{
    public class MetodoDB
    {
        private readonly string conexion = ConfigurationManager.ConnectionStrings["cadena_conexion"].ConnectionString;

        public int ObtenerIdPorNombre(string nombreMetodo)
        {
            int idMetodo = 0;

            using (var cn = new SQLiteConnection(conexion))
            {
                string query = "SELECT IdMetodo FROM METODO WHERE NombreMetodo = @nombreMetodo";
                using (var cmd = new SQLiteCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@nombreMetodo", nombreMetodo);

                    cn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        idMetodo = Convert.ToInt32(result);
                    }
                }
            }

            return idMetodo;
        }
    }
}
