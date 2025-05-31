using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMetodos.Models
{
    public class EjecucionGS
    {
        public int IdEjecucionGS { get; set; }
        public User oUsuario { get; set; }
        public Metodo oMetodo { get; set; }
        public string Ecuaciones { get; set; }
        public float X1_Final { get; set; }
        public float X2_Final { get; set; }
        public float X3_Final { get; set; }
        public int Iteraciones { get; set; }
        public float ErrorAproximado { get; set; }
        public string FechaRegistro { get; set; }
    }
}