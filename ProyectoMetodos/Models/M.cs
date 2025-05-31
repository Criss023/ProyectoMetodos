using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMetodos.Models
{
    public class M
    {
        public string Funcion { get; set; }
        public double X0 { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Tolerancia { get; set; }
        public int MaxIteraciones { get; set; }
    }
}