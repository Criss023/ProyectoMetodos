using System.ComponentModel.DataAnnotations;

namespace ProyectoMetodos.Models
{
    public class User
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(50, ErrorMessage = "Hasta 50 caracteres")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, ErrorMessage = "Hasta 100 caracteres")]
        public string Contrasena { get; set; }
    }

}
