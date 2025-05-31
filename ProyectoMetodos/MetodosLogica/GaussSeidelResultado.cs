using System;
using System.Collections.Generic;

namespace ProyectoMetodos.MetodosLogica
{
    public class GaussSeidelResultado
    {
        public List<IteracionGS> IteracionesDetalle { get; set; } = new List<IteracionGS>();

        public double X1
        {
            get
            {
                return IteracionesDetalle.Count > 0 ? IteracionesDetalle[IteracionesDetalle.Count - 1].X1 : 0;
            }
        }
        public double X2
        {
            get
            {
                return IteracionesDetalle.Count > 0 ? IteracionesDetalle[IteracionesDetalle.Count - 1].X2 : 0;
            }
        }
        public double X3
        {
            get
            {
                return IteracionesDetalle.Count > 0 ? IteracionesDetalle[IteracionesDetalle.Count - 1].X3 : 0;
            }
        }

        public int TotalIteraciones
        {
            get
            {
                return IteracionesDetalle.Count;
            }
        }

        public double Error
        {
            get
            {
                return IteracionesDetalle.Count > 0 ? IteracionesDetalle[IteracionesDetalle.Count - 1].Error : 0;
            }
        }

        public string MensajeError { get; set; }

        public class IteracionGS
        {
            public int Numero { get; set; }
            public double X1 { get; set; }
            public double X2 { get; set; }
            public double X3 { get; set; }
            public double Error { get; set; }
        }
    }
}
