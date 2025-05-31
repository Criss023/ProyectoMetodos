using ProyectoMetodos.MetodosLogica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMetodos.Models
{
    public class ReporteEjecucion
    {
        public string FuncionIngresada { get; set; }
        public string FechaEjecucion { get; set; }
        public List<MetodoResultado> Resultados { get; set; } = new List<MetodoResultado>();
        public List<IteracionMetodo> IteracionMetodos { get; set;} = new List<IteracionMetodo>();
    }
}