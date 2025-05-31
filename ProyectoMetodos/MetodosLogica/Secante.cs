using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace ProyectoMetodos.MetodosLogica
{
    public class Secante
    {
        public static List<MetodoResultado> Calcular(string funcion, double x0, double x1, double tolerancia, int maxIteraciones)
        {
            var resultados = new List<MetodoResultado>();

            try
            {
                var f = Expr.Parse(funcion);

                double error = double.MaxValue;
                int iteracion = 0;

                while (error > tolerancia && iteracion < maxIteraciones)
                {
                    var valoresX0 = new Dictionary<string, FloatingPoint> { { "x", x0 } };
                    var valoresX1 = new Dictionary<string, FloatingPoint> { { "x", x1 } };

                    double fx0 = f.Evaluate(valoresX0).RealValue;
                    double fx1 = f.Evaluate(valoresX1).RealValue;

                    if (fx1 - fx0 == 0)
                    {
                        resultados.Add(new MetodoResultado
                        {
                            Iteracion = iteracion,
                            MensajeError = "División por cero en la iteración " + iteracion
                        });
                        break;
                    }

                    double x2 = x1 - fx1 * (x1 - x0) / (fx1 - fx0);
                    error = Math.Abs(x2 - x1);

                    resultados.Add(new MetodoResultado
                    {
                        Iteracion = iteracion + 1,
                        X = x2,
                        Error = error
                    });

                    x0 = x1;
                    x1 = x2;
                    iteracion++;
                }

                if (iteracion == maxIteraciones)
                {
                    resultados.Add(new MetodoResultado
                    {
                        Iteracion = iteracion,
                        MensajeError = "Se alcanzó el número máximo de iteraciones"
                    });
                }
            }
            catch (Exception ex)
            {
                resultados.Add(new MetodoResultado
                {
                    Iteracion = 0,
                    MensajeError = "Error en el cálculo: " + ex.Message
                });
            }

            return resultados;
        }
    }
}
