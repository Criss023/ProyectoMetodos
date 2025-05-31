using System.Security.Cryptography;
using System.Text;

namespace ProyectoMetodos.Utilidades
{
    public class Seguridad
    {
        public static string HashearContrasena(string contrasena)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(contrasena);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2")); // convierte a hexadecimal
                }

                return sb.ToString();
            }
        }
    }
}
