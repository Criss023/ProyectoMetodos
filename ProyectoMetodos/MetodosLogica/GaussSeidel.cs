using System;
using System.Collections.Generic;

namespace ProyectoMetodos.MetodosLogica
{
    public class GaussSeidel
    {
        public static GaussSeidelResultado Calcular(double[,] A, double[] b, double[] x0, double tolerancia, int maxIteraciones)
        {
            int n = b.Length;
            double[] x = new double[n];
            Array.Copy(x0, x, n);

            var resultado = new GaussSeidelResultado(); // ✅ ahora sí está declarado

            double error = double.MaxValue;

            for (int iter = 1; iter <= maxIteraciones && error > tolerancia; iter++)
            {
                double[] xAnt = (double[])x.Clone();

                for (int i = 0; i < n; i++)
                {
                    double suma = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (j != i)
                            suma += A[i, j] * x[j];
                    }
                    x[i] = (b[i] - suma) / A[i, i];
                }

                error = 0;
                for (int i = 0; i < n; i++)
                    error += Math.Pow(x[i] - xAnt[i], 2);
                error = Math.Sqrt(error);

                resultado.IteracionesDetalle.Add(new GaussSeidelResultado.IteracionGS
                {
                    Numero = iter,
                    X1 = x[0],
                    X2 = x[1],
                    X3 = x[2],
                    Error = error
                });
            }

            return resultado;
        }
    }
}
