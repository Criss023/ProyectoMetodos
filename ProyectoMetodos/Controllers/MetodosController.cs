using ProyectoMetodos.MetodosLogica;
using ProyectoMetodos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SecanteLogica = ProyectoMetodos.MetodosLogica.Secante;
using NewtonLogica = ProyectoMetodos.MetodosLogica.NewtonRaphson;
using MullerLogica = ProyectoMetodos.MetodosLogica.Muller;
using GaussSeidelLogica = ProyectoMetodos.MetodosLogica.GaussSeidel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Web.Hosting;
using iTextSharp.tool.xml;
using System.Text;
using System.Web;
using Expr = MathNet.Symbolics.SymbolicExpression;
using System.Globalization;


namespace ProyectoMetodos.Controllers
{
    public class MetodosController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        // GET: Metodos/Newton Raphson
        [HttpGet]
        public ActionResult NewtonRaphson()
        {
            return View();
        }

        // Post: Metodos/Newton Raphson
        [HttpPost]
        public ActionResult NewtonRaphson(string funcion, double valorInicial, double tolerancia, int maxIteraciones)
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            var errores = new List<string>();

            // Validar función
            if (string.IsNullOrWhiteSpace(funcion))
            {
                errores.Add("Debe ingresar una función.");
            }
            else
            {
                try
                {
                    var expr = Expr.Parse(funcion); // prueba de parseo
                    var derivadaExpr = expr.Differentiate("x");
                }
                catch
                {
                    errores.Add("La función ingresada no es válida. Use notación correcta con 'x'.");
                }
            }

            // Validar tolerancia
            if (tolerancia <= 0)
                errores.Add("La tolerancia debe ser un valor positivo mayor a cero.");

            // Validar iteraciones
            if (maxIteraciones <= 0)
                errores.Add("El número de iteraciones debe ser mayor a cero.");

            if (errores.Any())
            {
                ViewBag.MetodoSeleccionado = "NewtonRaphson";
                ViewBag.FuncionIngresada = funcion;
                ViewBag.Errores = errores;
                return View("Index");
            }

            // Si todo está bien
            var exprFinal = Expr.Parse(funcion);
            var derivadaFinal = exprFinal.Differentiate("x").ToString();

            var resultados = NewtonLogica.Calcular(funcion, derivadaFinal, valorInicial, tolerancia, maxIteraciones);

            GuardarEjecucion("Newton-Raphson", funcion, resultados);

            Session["DerivadaActual"] = derivadaFinal;
            Session["ValorInicialActual"] = valorInicial;
            Session["ToleranciaActual"] = tolerancia;
            Session["MaxIterActual"] = maxIteraciones;
            Session["FuncionActual"] = funcion;
            Session["Resultados"] = resultados;
            Session["MetodoActual"] = "NewtonRaphson";

            ViewBag.Resultados = resultados;
            ViewBag.MetodoSeleccionado = "NewtonRaphson";
            ViewBag.FuncionIngresada = funcion;

            return View("Index");
        }


        // GET: Metodos/Secante
        [HttpGet]
        public ActionResult Secante()
        {
            return View();
        }

        // POST: Metodos/Secante
        [HttpPost]
        public ActionResult Secante(string funcion, double x0, double x1, double tolerancia, int maxIteraciones)
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(funcion))
            {
                errores.Add("Debe ingresar una función.");
            }
            else
            {
                try
                {
                    Expr.Parse(funcion);
                }
                catch
                {
                    errores.Add("La función ingresada no es válida. Use notación matemática correcta con 'x'.");
                }
            }

            if (tolerancia <= 0)
                errores.Add("La tolerancia debe ser mayor a cero.");

            if (maxIteraciones <= 0)
                errores.Add("El número de iteraciones debe ser mayor a cero.");

            if (x0 == x1)
                errores.Add("Los valores iniciales x0 y x1 deben ser diferentes.");

            if (errores.Any())
            {
                ViewBag.MetodoSeleccionado = "Secante";
                ViewBag.FuncionIngresada = funcion;
                ViewBag.Errores = errores;
                return View("Index");
            }

            var resultados = SecanteLogica.Calcular(funcion, x0, x1, tolerancia, maxIteraciones);

            GuardarEjecucion("Secante", funcion, resultados);

            Session["X0Actual"] = x0;
            Session["X1Actual"] = x1;
            Session["ToleranciaActual"] = tolerancia;
            Session["MaxIterActual"] = maxIteraciones;
            Session["FuncionActual"] = funcion;
            Session["Resultados"] = resultados;
            Session["MetodoActual"] = "Secante";
            ViewBag.Resultados = resultados;
            ViewBag.MetodoSeleccionado = "Secante";
            ViewBag.FuncionIngresada = funcion;

            return View("Index");
        }


        // GET: Metodos/Muller
        [HttpGet]
        public ActionResult Muller() 
        { 
            return View(); 
        }

        // POST: Metodos/Muller
        [HttpPost]
        public ActionResult Muller(string funcion, double x0, double x1, double x2, double tolerancia, int maxIteraciones)
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(funcion))
            {
                errores.Add("Debe ingresar una función.");
            }
            else
            {
                try
                {
                    Expr.Parse(funcion);
                }
                catch
                {
                    errores.Add("La función ingresada no es válida. Use notación correcta con 'x'.");
                }
            }

            if (tolerancia <= 0)
                errores.Add("La tolerancia debe ser mayor a cero.");

            if (maxIteraciones <= 0)
                errores.Add("El número de iteraciones debe ser mayor a cero.");

            if (x0 == x1 || x1 == x2 || x0 == x2)
                errores.Add("Los valores x0, x1 y x2 deben ser diferentes entre sí.");

            if (errores.Any())
            {
                ViewBag.MetodoSeleccionado = "Muller";
                ViewBag.FuncionIngresada = funcion;
                ViewBag.Errores = errores;
                return View("Index");
            }

            var resultados = MullerLogica.Calcular(funcion, x0, x1, x2, tolerancia, maxIteraciones);

            GuardarEjecucion("Muller", funcion, resultados);

            Session["X0Actual"] = x0;
            Session["X1Actual"] = x1;
            Session["X2Actual"] = x2;
            Session["ToleranciaActual"] = tolerancia;
            Session["MaxIterActual"] = maxIteraciones;
            Session["FuncionActual"] = funcion;
            Session["Resultados"] = resultados;
            Session["MetodoActual"] = "Muller";
            ViewBag.Resultados = resultados;
            ViewBag.MetodoSeleccionado = "Muller";
            ViewBag.FuncionIngresada = funcion;

            return View("Index");
        }


        // GET: Metodos/GaussSeidel
        [HttpGet]
        public ActionResult GaussSeidel()
        {
            return View();
        }

        // POST: Metodos/GaussSeidel
        [HttpPost]
        public ActionResult GaussSeidel(
     double A11, double A12, double A13,
     double A21, double A22, double A23,
     double A31, double A32, double A33,
     double b1, double b2, double b3,
     double x0_1, double x0_2, double x0_3,
     double tolerancia, int maxIteraciones)
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            var errores = new List<string>();

            // Validar que no haya filas completamente nulas
            bool fila1Cero = A11 == 0 && A12 == 0 && A13 == 0;
            bool fila2Cero = A21 == 0 && A22 == 0 && A23 == 0;
            bool fila3Cero = A31 == 0 && A32 == 0 && A33 == 0;

            if (fila1Cero || fila2Cero || fila3Cero)
                errores.Add("Las filas del sistema no pueden ser completamente cero.");

            if (tolerancia <= 0)
                errores.Add("La tolerancia debe ser mayor a cero.");

            if (maxIteraciones <= 0)
                errores.Add("El número de iteraciones debe ser mayor a cero.");

            if (errores.Any())
            {
                ViewBag.MetodoSeleccionado = "Gauss";
                ViewBag.Errores = errores;
                return View("Index");
            }

            double[,] A = new double[3, 3]
            {
        { A11, A12, A13 },
        { A21, A22, A23 },
        { A31, A32, A33 }
            };

            double[] b = new double[] { b1, b2, b3 };
            double[] x0 = new double[] { x0_1, x0_2, x0_3 };

            var resultados = GaussSeidelLogica.Calcular(A, b, x0, tolerancia, maxIteraciones);
            var ultimaIteracion = resultados.IteracionesDetalle.LastOrDefault();

            var ecuacionesList = GenerarEcuaciones(A, b);
            string ecuacionesTexto = GenerarTextoEcuaciones(ecuacionesList);

            if (ultimaIteracion != null)
            {
                GuardarEjecucionGaussSeidel("Gauss", ecuacionesTexto, resultados);
            }

            Session["ToleranciaActual"] = tolerancia;
            Session["MaxIterActual"] = maxIteraciones;
            Session["X1"] = x0_1;
            Session["X2"] = x0_2;
            Session["X3"] = x0_3;
            Session["EcuacionesList"] = ecuacionesList;
            Session["EcuacionesActual"] = ecuacionesTexto;
            Session["ResultadosGauss"] = resultados;
            Session["MetodoActual"] = "Gauss";
            ViewBag.ResultadosGauss = resultados;
            ViewBag.MetodoSeleccionado = "Gauss";
            ViewBag.FuncionIngresada = new MvcHtmlString(ecuacionesTexto);

            return View("Index");
        }



        // Método para guardar ejecución, reutilizable
        private void GuardarEjecucion<T>(string nombreMetodo, string funcion, List<T> resultados) where T : class
        {
            User usuarioSesion = (User)Session["usuario"];
            UsuarioDB usuarioDB = new UsuarioDB();
            MetodoDB metodoDB = new MetodoDB();
            EjecucionDB ejecucionDB = new EjecucionDB();

            int idUsuario = usuarioSesion.IdUsuario;
            int idMetodo = metodoDB.ObtenerIdPorNombre(nombreMetodo);

            // Tomar el último resultado
            dynamic resultadoFinal = resultados?.LastOrDefault();
            int iteraciones = resultados?.Count ?? 0;
            double errorAprox = resultadoFinal != null ? (double)resultadoFinal.Error : 0;

            Ejecucion ejecucion = new Ejecucion
            {
                oUsuario = new User { IdUsuario = idUsuario },
                oMetodo = new Metodo { IdMetodo = idMetodo },
                Ecuacion = funcion,
                Resultado = resultadoFinal != null ? resultadoFinal.X.ToString() : "Error",
                Iteraciones = iteraciones,
                ErrorAproximado = (float)errorAprox
            };

            int idEjecucion = ejecucionDB.GuardarEjecucion(ejecucion);

            if (idEjecucion > 0)
            {
                List<IteracionMetodo> listaIteraciones = new List<IteracionMetodo>();
                for (int i = 0; i < resultados.Count; i++)
                {
                    dynamic r = resultados[i];

                    listaIteraciones.Add(new IteracionMetodo
                    {
                        oMetodo = new Metodo { IdMetodo = idMetodo },
                        oEjecucion = new Ejecucion { IdEjecucion = idEjecucion },
                        Iteracion = i + 1,
                        ValorX = TienePropiedad(r, "X") ? (double?)r.X : null,
                        Error = TienePropiedad(r, "Error") ? (double)r.Error : 0
                    });
                }

                ejecucionDB.GuardarIteraciones(listaIteraciones);
            }
            else
            {
                ViewBag.ErrorGuardar = "Error al guardar la ejecución en la base de datos.";
            }

        }

        // Método auxiliar para comprobar propiedades
        private bool TienePropiedad(object obj, string nombreProp)
        {
            return obj.GetType().GetProperty(nombreProp) != null;
        }

        // Reporte
        [HttpGet]
        public ActionResult Reporte(string metodo)
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            var usuario = (User)Session["usuario"];

            if (metodo == "Gauss")
            {
                EjecucionGSDB ejecucionGSDB = new EjecucionGSDB();
                var ejecucionesGS = ejecucionGSDB.ListarPorUsuario(usuario.IdUsuario);

                if (ejecucionesGS.Any())
                {
                    ViewBag.MetodoSeleccionado = metodo;

                    List<ReporteGS> reportesGauss = new List<ReporteGS>();

                    foreach (var ej in ejecucionesGS)
                    {
                        var iteraciones = ejecucionGSDB.ObtenerIteracionesPorEjecucion(ej.IdEjecucionGS);
                        var reporte = new ReporteGS
                        {
                            FechaRegistro = ej.FechaRegistro,
                            Ecuaciones = ej.Ecuaciones,
                            Iteraciones = iteraciones
                        };
                        reportesGauss.Add(reporte);
                    }

                    ViewBag.ResultadosGauss = reportesGauss;
                    return View();
                }
                ViewBag.MetodoSeleccionado = metodo;
                ViewBag.ResultadosGauss = new List<ReporteGS>();
                return View();
            }

            else
            {
                EjecucionDB ejecucionDB = new EjecucionDB();

                List<Ejecucion> ejecuciones;

                if (metodo == "NewtonRaphson")
                {
                    ejecuciones = ejecucionDB.ListarPorUsuarioYMetodo(usuario.IdUsuario, 1);
                }
                else if (metodo == "Secante")
                {
                    ejecuciones = ejecucionDB.ListarPorUsuarioYMetodo(usuario.IdUsuario, 2);
                }
                else if (metodo == "Muller")
                {
                    ejecuciones = ejecucionDB.ListarPorUsuarioYMetodo(usuario.IdUsuario, 3);
                }
                else
                {
                    ejecuciones = new List<Ejecucion>();
                }

                if (ejecuciones.Any())
                {
                    ViewBag.FuncionIngresada = ejecuciones.First().Ecuacion;
                    ViewBag.MetodoSeleccionado = metodo;

                    List<ReporteEjecucion> reportes = new List<ReporteEjecucion>();

                    foreach (var ej in ejecuciones)
                    {
                        var resultado = new MetodoResultado
                        {
                            Iteracion = ej.Iteraciones,
                            X = double.TryParse(ej.Resultado, out double r) ? r : 0,
                            Error = ej.ErrorAproximado,
                        };

                        var iteraciones = ejecucionDB.ObtenerIteracionesPorEjecucion(ej.IdEjecucion);

                        reportes.Add(new ReporteEjecucion
                        {
                            FechaEjecucion = ej.FechaEjecucion,
                            FuncionIngresada = ej.Ecuacion,
                            Resultados = new List<MetodoResultado> { resultado },
                            IteracionMetodos = iteraciones 
                        });
                    }

                    ViewBag.Reportes = reportes;

                    return View();
                }


                ViewBag.MetodoSeleccionado = metodo;
                ViewBag.Resultados = new List<MetodoResultado>();
                return View();
            }
        }



        // Guardar Resultado de Gauss
        private void GuardarEjecucionGaussSeidel(string nombreMetodo, string ecuaciones, GaussSeidelResultado resultado)
        {
            User usuarioSesion = (User)Session["usuario"];
            UsuarioDB usuarioDB = new UsuarioDB();
            MetodoDB metodoDB = new MetodoDB();
            EjecucionGSDB ejecucionGaussDB = new EjecucionGSDB();

            int idUsuario = usuarioSesion.IdUsuario;
            int idMetodo = metodoDB.ObtenerIdPorNombre(nombreMetodo);

            EjecucionGS ejecucion = new EjecucionGS
            {
                oUsuario = new User { IdUsuario = idUsuario },
                oMetodo = new Metodo { IdMetodo = idMetodo },
                Ecuaciones = ecuaciones,
                X1_Final = (float)resultado.X1,
                X2_Final = (float)resultado.X2,
                X3_Final = (float)resultado.X3,
                Iteraciones = resultado.TotalIteraciones,
                ErrorAproximado = (float)resultado.Error
            };

            bool guardado = ejecucionGaussDB.GuardarEjecucionGaussSeidel(ejecucion, resultado.IteracionesDetalle);
            if (!guardado)
                ViewBag.ErrorGuardar = "Error al guardar la ejecución de Gauss-Seidel.";
        }

        // Método para generar ecuaciones como lista de coeficientes
        private List<double[]> GenerarEcuaciones(double[,] A, double[] b)
        {
            int n = b.Length;
            var ecuacionesList = new List<double[]>();

            for (int i = 0; i < n; i++)
            {
                var ecuacion = new double[4];
                ecuacion[0] = A[i, 0];  // Coeficiente x1
                ecuacion[1] = A[i, 1];  // Coeficiente x2
                ecuacion[2] = A[i, 2];  // Coeficiente x3
                ecuacion[3] = b[i];     // Término independiente
                ecuacionesList.Add(ecuacion);
            }

            return ecuacionesList;
        }

        // Método para generar texto de ecuaciones (para la vista)
        private string GenerarTextoEcuaciones(List<double[]> ecuacionesList)
        {
            var sb = new StringBuilder();
            foreach (var ecuacion in ecuacionesList)
            {
                // Formatear cada término con su signo
                sb.Append(FormatearTermino(ecuacion[0], "x1", true));
                sb.Append(FormatearTermino(ecuacion[1], "x2", false));
                sb.Append(FormatearTermino(ecuacion[2], "x3", false));
                sb.Append($" = {ecuacion[3]}");
                sb.AppendLine("<br/>");
            }
            return sb.ToString();
        }

        // Formatear un término individual
        private string FormatearTermino(double coeficiente, string variable, bool esPrimerTermino)
        {
            if (coeficiente == 0) return string.Empty;

            string signo = "";
            string valorAbsoluto = Math.Abs(coeficiente).ToString("0.##", CultureInfo.InvariantCulture);

            // Eliminar ".00" si es número entero
            if (valorAbsoluto.EndsWith(".00"))
                valorAbsoluto = valorAbsoluto.Substring(0, valorAbsoluto.Length - 3);
            else if (valorAbsoluto.EndsWith(".0"))
                valorAbsoluto = valorAbsoluto.Substring(0, valorAbsoluto.Length - 2);

            if (!esPrimerTermino)
            {
                signo = coeficiente >= 0 ? " + " : " - ";
            }
            else
            {
                signo = coeficiente >= 0 ? "" : "-";
            }

            // Manejar coeficiente 1 o -1 para no mostrar el número
            if (Math.Abs(coeficiente) == 1)
            {
                return $"{signo}{variable}";
            }

            return $"{signo}{valorAbsoluto}{variable}";
        }

        // Guardar PDF
        [HttpGet]
        public ActionResult DescargarPDF()
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            // 1. Obtener el método y ruta de plantilla
            string metodo = Session["MetodoActual"] as string;
            if (string.IsNullOrEmpty(metodo))
                return Content("No hay ejecución reciente para descargar.");

            string plantillaFile;
            if (metodo == "NewtonRaphson")
                plantillaFile = "PlantillaNR.html";
            else if (metodo == "Secante")
                plantillaFile = "PlantillaS.html";
            else if (metodo == "Muller")
                plantillaFile = "PlantillaM.html";
            else if (metodo == "Gauss")
                plantillaFile = "PlantillaGS.html";
            else
                throw new InvalidOperationException("Método no soportado");


            string rutaPlantilla = HostingEnvironment.MapPath($"~/Content/plantillas/{plantillaFile}");
            if (!System.IO.File.Exists(rutaPlantilla))
                return Content("Plantilla no encontrada.");

            // 2. Leer plantilla y reemplazar marcadores básicos
            double tolerancia = Session["ToleranciaActual"] is double t ? t : 0;
            string toleranciaStr = tolerancia.ToString("F6");    

         
            string html = System.IO.File.ReadAllText(rutaPlantilla)
                .Replace("@metodo", metodo.ToUpper())
                .Replace("@fecha", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                .Replace("@funcion", HttpUtility.HtmlEncode(Session["FuncionActual"] as string ?? ""))
                .Replace("@tolerancia", toleranciaStr)
                .Replace("@maxIteraciones", Session["MaxIterActual"]?.ToString() ?? "");

            // 3. Reemplazar parámetros específicos y construir filas
            var sb = new StringBuilder();

            switch (metodo)
            {
                case "NewtonRaphson":
                    html = html
                        .Replace("@derivada", HttpUtility.HtmlEncode(Session["DerivadaActual"] as string ?? ""))
                        .Replace("@x0", Session["ValorInicialActual"]?.ToString() ?? "");
                    var resNR = Session["Resultados"] as List<MetodoResultado>;
                    foreach (var r in resNR)
                        sb.AppendFormat("<tr><td>{0}</td><td>{1:F6}</td><td>{2:F6}</td></tr>",
                                        r.Iteracion, r.X, r.Error);
                    html = html.Replace("@filas", sb.ToString());
                    break;

                case "Secante":
                    html = html
                        .Replace("@x0", Session["X0Actual"]?.ToString() ?? "")
                        .Replace("@x1", Session["X1Actual"]?.ToString() ?? "");
                    var resS = Session["Resultados"] as List<MetodoResultado>;
                    foreach (var r in resS)
                        sb.AppendFormat("<tr><td>{0}</td><td>{1:F6}</td><td>{2:F6}</td></tr>",
                                        r.Iteracion, r.X, r.Error);
                    html = html.Replace("@filas", sb.ToString());
                    break;

                case "Muller":
                    html = html
                        .Replace("@x0", Session["X0Actual"]?.ToString() ?? "")
                        .Replace("@x1", Session["X1Actual"]?.ToString() ?? "")
                        .Replace("@x2", Session["X2Actual"]?.ToString() ?? "");
                    var resM = Session["Resultados"] as List<MetodoResultado>;
                    foreach (var r in resM)
                        sb.AppendFormat("<tr><td>{0}</td><td>{1:F6}</td><td>{2:F6}</td></tr>",
                                        r.Iteracion, r.X, r.Error);
                    html = html.Replace("@filas", sb.ToString());
                    break;

                case "Gauss":
                    var resultadoGS = Session["ResultadosGauss"] as GaussSeidelResultado;
                    var ecuacionesList = Session["EcuacionesList"] as List<double[]>;

                    var sbEcuaciones = new StringBuilder();
                    if (ecuacionesList != null)
                    {
                        foreach (var ecuacion in ecuacionesList)
                        {
                            sbEcuaciones.Append("<tr>");

                            // Primer término
                            sbEcuaciones.Append($"<td>{ecuacion[0].ToString(CultureInfo.InvariantCulture)}</td>");
                            sbEcuaciones.Append("<td class=\"variable\">x1</td>");

                            // Segundo término
                            string signo2 = ecuacion[1] >= 0 ? "+" : "-";
                            sbEcuaciones.Append($"<td class=\"operator\">{signo2}</td>");
                            sbEcuaciones.Append($"<td>{Math.Abs(ecuacion[1]).ToString(CultureInfo.InvariantCulture)}</td>");
                            sbEcuaciones.Append("<td class=\"variable\">x2</td>");

                            // Tercer término
                            string signo3 = ecuacion[2] >= 0 ? "+" : "-";
                            sbEcuaciones.Append($"<td class=\"operator\">{signo3}</td>");
                            sbEcuaciones.Append($"<td>{Math.Abs(ecuacion[2]).ToString(CultureInfo.InvariantCulture)}</td>");
                            sbEcuaciones.Append("<td class=\"variable\">x3</td>");

                            // Igual
                            sbEcuaciones.Append("<td class=\"equals\">=</td>");

                            // Término independiente
                            sbEcuaciones.Append($"<td>{ecuacion[3].ToString(CultureInfo.InvariantCulture)}</td>");

                            sbEcuaciones.Append("</tr>");
                        }
                    }

                    html = html
                        .Replace("@x1", Session["X1"]?.ToString() ?? "")
                        .Replace("@x2", Session["X2"]?.ToString() ?? "")
                        .Replace("@x3", Session["X3"]?.ToString() ?? "")
                        .Replace("@ecuacionesTable", sbEcuaciones.ToString());
                    
                    // Generar filas de iteraciones
                    var sbGS = new StringBuilder();
                    foreach (var it in resultadoGS.IteracionesDetalle)
                    {
                        sbGS.AppendFormat("<tr><td>{0}</td><td>{1:F6}</td><td>{2:F6}</td><td>{3:F6}</td><td>{4:F6}</td></tr>",
                                          it.Numero, it.X1, it.X2, it.X3, it.Error);
                    }
                    html = html.Replace("@filasIteraciones", sbGS.ToString());
                    break;
            }

            // 4. Generar PDF
            var ms = new MemoryStream();
            var doc = new Document(PageSize.A4, 25, 25, 25, 25);
            var writer = PdfWriter.GetInstance(doc, ms);
            writer.CloseStream = false;
            doc.Open();

            // Parsear HTML
            using (var sr = new StringReader(html))
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);

            doc.Close();

            // 5. Devolver bytes
            byte[] pdfBytes = ms.ToArray();
            return File(pdfBytes, "application/pdf", $"Reporte_{metodo}.pdf");
        }

    }
}
