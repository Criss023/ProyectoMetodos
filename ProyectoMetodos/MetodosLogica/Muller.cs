using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;
using System;
using System.Collections.Generic;

namespace ProyectoMetodos.MetodosLogica
{
    public class Muller
    {
        public static List<MetodoResultado> Calcular(string funcionStr, double x0, double x1, double x2, double tolerancia, int maxIteraciones)
        {
            var resultados = new List<MetodoResultado>();

            try
            {
                Expr funcion = Expr.Parse(funcionStr);
                double error = double.MaxValue;
                int iteracion = 1;

                while (error > tolerancia && iteracion <= maxIteraciones)
                {
                    // Evaluar f(x0), f(x1), f(x2)
                    double f0 = funcion.Evaluate(new Dictionary<string, FloatingPoint> { { "x", x0 } }).RealValue;
                    double f1 = funcion.Evaluate(new Dictionary<string, FloatingPoint> { { "x", x1 } }).RealValue;
                    double f2 = funcion.Evaluate(new Dictionary<string, FloatingPoint> { { "x", x2 } }).RealValue;

                    double h1 = x1 - x0;
                    double h2 = x2 - x1;

                    double d1 = (f1 - f0) / h1;
                    double d2 = (f2 - f1) / h2;

                    double a = (d2 - d1) / (h2 + h1);
                    double b = a * h2 + d2;
                    double c = f2;

                    double discriminante = b * b - 4 * a * c;

                    if (discriminante < 0)
                    {
                        resultados.Add(new MetodoResultado
                        {
                            Iteracion = iteracion,
                            MensajeError = "Raíz compleja encontrada. Método detenido."
                        });
                        break;
                    }

                    discriminante = Math.Sqrt(discriminante);
                    double denominador = Math.Abs(b + discriminante) > Math.Abs(b - discriminante) ? b + discriminante : b - discriminante;

                    if (denominador == 0)
                    {
                        resultados.Add(new MetodoResultado
                        {
                            Iteracion = iteracion,
                            MensajeError = "División por cero. Método detenido."
                        });
                        break;
                    }

                    double dxr = -2 * c / denominador;
                    double xr = x2 + dxr;

                    error = Math.Abs(dxr);

                    resultados.Add(new MetodoResultado
                    {
                        Iteracion = iteracion,
                        X = xr,
                        Error = error
                    });

                    // Preparar para la siguiente iteración
                    x0 = x1;
                    x1 = x2;
                    x2 = xr;

                    iteracion++;
                }
            }
            catch (Exception ex)
            {
                resultados.Add(new MetodoResultado
                {
                    MensajeError = $"Error: {ex.Message}"
                });
            }

            return resultados;
        }
    }
}
