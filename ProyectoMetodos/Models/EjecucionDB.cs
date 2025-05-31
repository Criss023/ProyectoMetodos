using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using ProyectoMetodos.Models;

namespace ProyectoMetodos.Models
{
    public class EjecucionDB
    {
        private readonly string conexion = ConfigurationManager.ConnectionStrings["cadena_conexion"].ConnectionString;

        public int GuardarEjecucion(Ejecucion ejecucion)
        {
            using (var cn = new SQLiteConnection(conexion))
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(@"
            INSERT INTO EJECUCION
              (IdUsuario, IdMetodo, Ecuacion, Resultado, Iteraciones, ErrorAproximado, FechaEjecucion)
            VALUES
              (@IdUsuario, @IdMetodo, @Ecuacion, @Resultado, @Iteraciones, @ErrorAproximado, @FechaEjecucion);
            SELECT last_insert_rowid();", cn))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", ejecucion.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("@IdMetodo", ejecucion.oMetodo.IdMetodo);
                    cmd.Parameters.AddWithValue("@Ecuacion", ejecucion.Ecuacion);
                    cmd.Parameters.AddWithValue("@Resultado", ejecucion.Resultado);
                    cmd.Parameters.AddWithValue("@Iteraciones", ejecucion.Iteraciones);
                    cmd.Parameters.AddWithValue("@ErrorAproximado", ejecucion.ErrorAproximado);
                    cmd.Parameters.AddWithValue("@FechaEjecucion", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    object o = cmd.ExecuteScalar();
                    if (o != null && int.TryParse(o.ToString(), out int newId))
                        return newId;

                    return 0; 
                }
            }
        }


        public bool GuardarIteraciones(List<IteracionMetodo> iteraciones)
        {
            using (var cn = new SQLiteConnection(conexion))
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    foreach (var iter in iteraciones)
                    {
                        using (var cmd = new SQLiteCommand(@"
                            INSERT INTO ITERACION_METODO
                            (IdMetodo, IdEjecucion, Iteracion, ValorX, Error)
                            VALUES (@IdMetodo, @IdEjecucion, @Iteracion, @ValorX, @Error)", cn))
                        {
                            cmd.Parameters.AddWithValue("@IdMetodo", iter.oMetodo.IdMetodo);
                            cmd.Parameters.AddWithValue("@IdEjecucion", iter.oEjecucion.IdEjecucion);
                            cmd.Parameters.AddWithValue("@Iteracion", iter.Iteracion);
                            cmd.Parameters.AddWithValue("@ValorX", iter.ValorX.HasValue ? (object)iter.ValorX.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@Error", iter.Error);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                return true;
            }
        }

        public List<Ejecucion> ListarPorUsuarioYMetodo(int idUsuario, int idMetodo)
        {
            var lista = new List<Ejecucion>();
            using (var cn = new SQLiteConnection(conexion))
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(@"
                    SELECT E.IdEjecucion, E.Ecuacion, E.Resultado, E.Iteraciones, E.ErrorAproximado, E.FechaEjecucion,
                           M.IdMetodo, M.NombreMetodo
                    FROM EJECUCION E
                    JOIN METODO M ON E.IdMetodo = M.IdMetodo
                    WHERE E.IdUsuario = @idUsuario AND E.IdMetodo = @idMetodo
                    ORDER BY E.FechaEjecucion DESC", cn))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@idMetodo", idMetodo);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Ejecucion
                            {
                                IdEjecucion = Convert.ToInt32(dr["IdEjecucion"]),
                                Ecuacion = dr["Ecuacion"].ToString(),
                                Resultado = dr["Resultado"].ToString(),
                                Iteraciones = Convert.ToInt32(dr["Iteraciones"]),
                                ErrorAproximado = Convert.ToSingle(dr["ErrorAproximado"]),
                                FechaEjecucion = dr["FechaEjecucion"].ToString(),
                                oMetodo = new Metodo
                                {
                                    IdMetodo = Convert.ToInt32(dr["IdMetodo"]),
                                    NombreMetodo = dr["NombreMetodo"].ToString()
                                }
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public List<IteracionMetodo> ObtenerIteracionesPorEjecucion(int idEjecucion)
        {
            var lista = new List<IteracionMetodo>();
            using (var cn = new SQLiteConnection(conexion))
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(@"
                    SELECT Iteracion, ValorX, Error
                    FROM ITERACION_METODO
                    WHERE IdEjecucion = @IdEjecucion
                    ORDER BY Iteracion ASC", cn))
                {
                    cmd.Parameters.AddWithValue("@IdEjecucion", idEjecucion);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new IteracionMetodo
                            {
                                Iteracion = Convert.ToInt32(dr["Iteracion"]),
                                ValorX = dr["ValorX"] != DBNull.Value ? (double?)Convert.ToDouble(dr["ValorX"]) : null,
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
