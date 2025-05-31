using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMetodos.Models
{
    public class IteracionMetodo
    {
        public int IdIteracion { get; set; }
        public Metodo oMetodo { get; set; }
        public Ejecucion oEjecucion { get; set; }
        public int Iteracion { get; set; }
        public double? ValorX { get; set; }
        public double Error { get; set; }
    }

}