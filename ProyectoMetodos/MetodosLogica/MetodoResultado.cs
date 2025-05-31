using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMetodos.MetodosLogica
{
    public class MetodoResultado
    {
        public int Iteracion { get; set; }
        public double X { get; set; }
        public double Error { get; set; }
        public string MensajeError { get; set; }
    }
}