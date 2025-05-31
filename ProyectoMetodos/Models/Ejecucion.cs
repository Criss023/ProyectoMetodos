using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMetodos.Models
{
    public class Ejecucion
    {
        public int IdEjecucion { get; set; }
        public User oUsuario { get; set; }
        public Metodo oMetodo { get; set; }
        public string Ecuacion { get; set; }
        public string Resultado { get; set; }
        public int Iteraciones { get; set; }
        public float ErrorAproximado { get; set; }
        public string FechaEjecucion { get; set; }
    }
}