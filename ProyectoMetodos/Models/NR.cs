using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMetodos.Models
{
    public class NR
    {
        public string Funcion { get; set; }
        public string Derivada { get; set; }
        public double ValorInicial { get; set; }
        public double Tolerancia { get; set; }
        public int MaxIteraciones { get; set; }
    }
}