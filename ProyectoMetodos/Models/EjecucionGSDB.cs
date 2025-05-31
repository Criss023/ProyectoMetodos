using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using ProyectoMetodos.MetodosLogica;
using ProyectoMetodos.Models;

namespace ProyectoMetodos.Models
{
    public class EjecucionGSDB
    {
        private readonly string conexion = ConfigurationManager.ConnectionStrings["cadena_conexion"].ConnectionString;

        public bool GuardarEjecucionGaussSeidel(EjecucionGS ejecucion, List<GaussSeidelResultado.IteracionGS> iteraciones)
        {
            using (var cn = new SQLiteConnection(conexion))
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        // 1. Insertar ejecución y obtener Id generado
                        using (var cmd = new SQLiteCommand(@"
                            INSERT INTO EJECUCION_GAUSSSEIDEL
                            (IdUsuario, IdMetodo, Ecuaciones, X1_Final, X2_Final, X3_Final, Iteraciones, ErrorAproximado, FechaRegistro)
                            VALUES
                            (@IdUsuario, @IdMetodo, @Ecuaciones, @X1, @X2, @X3, @Iteraciones, @ErrorAproximado, @FechaRegistro);
                            SELECT last_insert_rowid();", cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@IdUsuario", ejecucion.oUsuario.IdUsuario);
                            cmd.Parameters.AddWithValue("@IdMetodo", ejecucion.oMetodo.IdMetodo);
                            cmd.Parameters.AddWithValue("@Ecuaciones", ejecucion.Ecuaciones);
                            cmd.Parameters.AddWithValue("@X1", ejecucion.X1_Final);
                            cmd.Parameters.AddWithValue("@X2", ejecucion.X2_Final);
                            cmd.Parameters.AddWithValue("@X3", ejecucion.X3_Final);
                            cmd.Parameters.AddWithValue("@Iteraciones", ejecucion.Iteraciones);
                            cmd.Parameters.AddWithValue("@ErrorAproximado", ejecucion.ErrorAproximado);
                            cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));


                            long id = (long)cmd.ExecuteScalar();
                            ejecucion.IdEjecucionGS = (int)id;
                        }

                        // 2. Insertar iteraciones
                        foreach (var iter in iteraciones)
                        {
                            using (var cmdIter = new SQLiteCommand(@"
                                INSERT INTO ITERACION_GAUSSSEIDEL
                                (IdEjecucionGS, Iteracion, X1, X2, X3, Error)
                                VALUES
                                (@IdGS, @Iteracion, @X1, @X2, @X3, @Error)", cn, transaction))
                            {
                                cmdIter.Parameters.AddWithValue("@IdGS", ejecucion.IdEjecucionGS);
                                cmdIter.Parameters.AddWithValue("@Iteracion", iter.Numero);
                                cmdIter.Parameters.AddWithValue("@X1", iter.X1);
                                cmdIter.Parameters.AddWithValue("@X2", iter.X2);
                                cmdIter.Parameters.AddWithValue("@X3", iter.X3);
                                cmdIter.Parameters.AddWithValue("@Error", iter.Error);
                                cmdIter.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public List<EjecucionGS> ListarPorUsuario(int idUsuario)
        {
            var lista = new List<EjecucionGS>();
            using (var cn = new SQLiteConnection(conexion))
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(@"
                    SELECT IdEjecucionGS, Ecuaciones, X1_Final, X2_Final, X3_Final, Iteraciones, ErrorAproximado, FechaRegistro
                    FROM EJECUCION_GAUSSSEIDEL
                    WHERE IdUsuario = @IdUsuario
                    ORDER BY FechaRegistro DESC", cn))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new EjecucionGS
                            {
                                IdEjecucionGS = Convert.ToInt32(dr["IdEjecucionGS"]),
                                Ecuaciones = dr["Ecuaciones"].ToString(),
                                X1_Final = Convert.ToSingle(dr["X1_Final"]),
                                X2_Final = Convert.ToSingle(dr["X2_Final"]),
                                X3_Final = Convert.ToSingle(dr["X3_Final"]),
                                Iteraciones = Convert.ToInt32(dr["Iteraciones"]),
                                ErrorAproximado = Convert.ToSingle(dr["ErrorAproximado"]),
                                FechaRegistro = dr["FechaRegistro"].ToString(),
                                oMetodo = new Metodo { IdMetodo = 4, NombreMetodo = "Gauss-Seidel" }
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public List<GaussSeidelResultado.IteracionGS> ObtenerIteracionesPorEjecucion(int idEjecucionGS)
        {
            var lista = new List<GaussSeidelResultado.IteracionGS>();
            using (var cn = new SQLiteConnection(conexion))
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(@"
                    SELECT Iteracion, X1, X2, X3, Error
                    FROM ITERACION_GAUSSSEIDEL
                    WHERE IdEjecucionGS = @IdGS
                    ORDER BY Iteracion ASC", cn))
                {
                    cmd.Parameters.AddWithValue("@IdGS", idEjecucionGS);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new GaussSeidelResultado.IteracionGS
                            {
                                Numero = Convert.ToInt32(dr["Iteracion"]),
                                X1 = Convert.ToDouble(dr["X1"]),
                                X2 = Convert.ToDouble(dr["X2"]),
                                X3 = Convert.ToDouble(dr["X3"]),
                                Error = Convert.ToDouble(dr["Error"])
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}
