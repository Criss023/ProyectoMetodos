using ProyectoMetodos.MetodosLogica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMetodos.Models
{
    public class ReporteGS
    {
        public string Ecuaciones { get; set; }
        public List<GaussSeidelResultado.IteracionGS> Iteraciones { get; set; } = new List<GaussSeidelResultado.IteracionGS>();
        public string FechaRegistro { get; set; }
    }
}