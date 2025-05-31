using System;
using System.Configuration;
using System.Data.SQLite;

namespace ProyectoMetodos.Models
{
    public class UsuarioDB
    {
        private readonly string conexion = ConfigurationManager.ConnectionStrings["cadena_conexion"].ConnectionString;

        public User ValidarUsuario(string usuario, string contrasenaHasheada)
        {
            User usuarioEncontrado = null;

            using (var cn = new SQLiteConnection(conexion))
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(@"
                    SELECT IdUsuario, Usuario, Contrasena
                    FROM USUARIO
                    WHERE Usuario = @user
                      AND Contrasena = @contrasena", cn))
                {
                    cmd.Parameters.AddWithValue("@user", usuario);
                    cmd.Parameters.AddWithValue("@contrasena", contrasenaHasheada);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuarioEncontrado = new User
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Usuario = dr["Usuario"].ToString(),
                                Contrasena = dr["Contrasena"].ToString()
                            };
                        }
                    }
                }
            }

            return usuarioEncontrado;
        }

        public int ObtenerIdPorContrasena(string contrasena)
        {
            int idUsuario = 0;

            using (var cn = new SQLiteConnection(conexion))
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(
                    "SELECT IdUsuario FROM USUARIO WHERE Contrasena = @contrasena", cn))
                {
                    cmd.Parameters.AddWithValue("@contrasena", contrasena);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        idUsuario = Convert.ToInt32(result);
                }
            }

            return idUsuario;
        }

        public bool RegistrarUsuario(User usuario)
        {
            using (var cn = new SQLiteConnection(conexion))
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(@"
                    INSERT INTO USUARIO (Usuario, Contrasena)
                    VALUES (@Usuario, @Contrasena)", cn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                    cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
