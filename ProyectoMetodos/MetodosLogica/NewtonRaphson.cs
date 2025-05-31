using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;
using System.Collections.Generic;
using System.Globalization;
using System;

namespace ProyectoMetodos.MetodosLogica
{
    public class NewtonRaphson
    {
        public static List<MetodoResultado> Calcular(string funcionStr, string derivadaStr, double x0, double tolerancia, int maxIteraciones)
        {
            var resultados = new List<MetodoResultado>();

            try
            {
                Expr funcion = Expr.Parse(funcionStr);
                Expr derivada = Expr.Parse(derivadaStr);

                double xActual = x0;

                for (int i = 1; i <= maxIteraciones; i++)
                {
                    var valores = new Dictionary<string, FloatingPoint> { { "x", xActual } };

                    double fx = funcion.Evaluate(valores).RealValue;
                    double dfx = derivada.Evaluate(valores).RealValue;

                    if (Math.Abs(dfx) < 1e-10)
                    {
                        resultados.Add(new MetodoResultado
                        {
                            Iteracion = i,
                            MensajeError = "Derivada cercana a cero, no se puede continuar."
                        });
                        break;
                    }

                    double xNuevo = xActual - fx / dfx;
                    double error = Math.Abs(xNuevo - xActual);

                    resultados.Add(new MetodoResultado
                    {
                        Iteracion = i,
                        X = xNuevo,
                        Error = error
                    });

                    if (error < tolerancia)
                        break;

                    xActual = xNuevo;
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
