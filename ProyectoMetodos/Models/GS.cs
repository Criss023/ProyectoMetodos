using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMetodos.Models
{
    public class GS
    {
        public double[,] MatrizCoeficientes { get; set; } 
        public double[] VectorConstantes { get; set; }   
        public double[] ValoresIniciales { get; set; }   
        public double Tolerancia { get; set; }
        public int MaxIteraciones { get; set; }
    }
}