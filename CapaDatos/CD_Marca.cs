using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Marca
    {

        public List<Marca> Listar()
        {

            List<Marca> lista = new List<Marca>();
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT MAR_ID, MAR_DESCRIPCION, MAR_ACTIVO FROM MARCA";
                    SqlCommand cmd = new SqlCommand(query, conex);
                    cmd.CommandType = CommandType.Text;

                    conex.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Marca()
                                {
                                    MAR_ID = Convert.ToInt32(dr["MAR_ID"]),
                                    MAR_DESCRIPCION = dr["MAR_DESCRIPCION"].ToString(),
                                    MAR_ACTIVO = Convert.ToBoolean(dr["MAR_ACTIVO"])
                                });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Marca>();
            }

            return lista;

        }

        public int Registrar(Marca obj, out string mensaje)
        {
            int idAutugenerado = 0;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRAR_MARCA", conex);

                    cmd.Parameters.AddWithValue("@MAR_DESCRIPCION", obj.MAR_DESCRIPCION);
                    cmd.Parameters.AddWithValue("@MAR_ACTIVO", obj.MAR_ACTIVO);
                    cmd.Parameters.AddWithValue("@RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;// significa que esta variable se llenara en la BD, no es parametro de entrada
                    cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conex.Open();
                    cmd.ExecuteNonQuery();

                    idAutugenerado = Convert.ToInt32(cmd.Parameters["@RESULTADO"].Value);
                    mensaje = cmd.Parameters["@MENSAJE"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idAutugenerado = 0;
                mensaje = ex.Message;
            }
            return idAutugenerado;
        }

        public bool Editar(Marca obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_MARCA", conex);

                    cmd.Parameters.AddWithValue("@MAR_ID", obj.MAR_ID);
                    cmd.Parameters.AddWithValue("@MAR_DESCRIPCION", obj.MAR_DESCRIPCION);
                    cmd.Parameters.AddWithValue("@MAR_ACTIVO", obj.MAR_ACTIVO);
                    cmd.Parameters.AddWithValue("@RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;// significa que esta variable se llenara en la BD, no es parametro de entrada
                    cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conex.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@RESULTADO"].Value);
                    mensaje = cmd.Parameters["@MENSAJE"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }

        public bool Eliminar(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_MARCA", conex);

                    cmd.Parameters.AddWithValue("@MAR_ID", id);
                    cmd.Parameters.AddWithValue("@RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;// significa que esta variable se llenara en la BD, no es parametro de entrada
                    cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conex.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@RESULTADO"].Value);
                    mensaje = cmd.Parameters["@MENSAJE"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }

        public List<Marca> ListarMarcaPorCategoria(int idcategoria)
        {

            List<Marca> lista = new List<Marca>();
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT DISTINCT M.MAR_ID, M.MAR_DESCRIPCION FROM PRODUCTO P");
                    sb.AppendLine("INNER JOIN CATEGORIA C ON C.CAT_ID = P.PROD_CAT_ID");
                    sb.AppendLine("INNER JOIN MARCA M ON M.MAR_ID = P.PROD_MAR_ID AND M.MAR_ACTIVO = 1");
                    sb.AppendLine("WHERE C.CAT_ID = IIF(@IDCATEGORIA = 0, C.CAT_ID, @IDCATEGORIA)");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), conex);
                    cmd.Parameters.AddWithValue("@IDCATEGORIA", idcategoria);
                    cmd.CommandType = CommandType.Text;

                    conex.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Marca()
                                {
                                    MAR_ID = Convert.ToInt32(dr["MAR_ID"]),
                                    MAR_DESCRIPCION = dr["MAR_DESCRIPCION"].ToString()
                                });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Marca>();
            }

            return lista;

        }

    }
}
