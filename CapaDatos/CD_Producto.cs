using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using CapaEntidad;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Producto
    {

        public List<Producto> Listar()
        {

            List<Producto> lista = new List<Producto>();
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT P.PROD_ID, P.PROD_NOMBRE, P.PROD_DESCRIPCION,");
                    sb.AppendLine("M.MAR_ID, M.MAR_DESCRIPCION,");
                    sb.AppendLine("C.CAT_ID, C.CAT_DESCRIPCION,");
                    sb.AppendLine("P.PROD_PRECIO, P.PROD_STOCK, P.PROD_RUTAIMAGEN, P.PROD_NOMBREIMAGEN, P.PROD_ACTIVO");
                    sb.AppendLine("FROM PRODUCTO P");
                    sb.AppendLine("INNER JOIN MARCA M ON M.MAR_ID = P.PROD_MAR_ID");
                    sb.AppendLine("INNER JOIN CATEGORIA C ON C.CAT_ID = P.PROD_CAT_ID");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), conex);
                    cmd.CommandType = CommandType.Text;

                    conex.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Producto()
                                {
                                    PROD_ID = Convert.ToInt32(dr["PROD_ID"]),
                                    PROD_NOMBRE = dr["PROD_NOMBRE"].ToString(),
                                    PROD_DESCRIPCION = dr["PROD_DESCRIPCION"].ToString(),
                                    oMarca = new Marca() { MAR_ID = Convert.ToInt32(dr["MAR_ID"]), MAR_DESCRIPCION = dr["MAR_DESCRIPCION"].ToString() },
                                    oCategoria = new Categoria() { CAT_ID = Convert.ToInt32(dr["CAT_ID"]), CAT_DESCRIPCION = dr["CAT_DESCRIPCION"].ToString() },
                                    PROD_PRECIO =Convert.ToDecimal(dr["PROD_PRECIO"], new CultureInfo("es-MX")),
                                    PROD_STOCK = Convert.ToInt32(dr["PROD_STOCK"]),
                                    PROD_IMAGEN = dr["PROD_RUTAIMAGEN"].ToString(),
                                    PROD_NOMBREIMAGEN = dr["PROD_NOMBREIMAGEN"].ToString(),
                                    PROD_ACTIVO = Convert.ToBoolean(dr["PROD_ACTIVO"])
                                });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Producto>();
            }

            return lista;

        }

        public int Registrar(Producto obj, out string mensaje)
        {
            int idAutugenerado = 0;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRAR_PRODUCTO", conex);

                    cmd.Parameters.AddWithValue("@PROD_DESCRIPCION", obj.PROD_DESCRIPCION);
                    cmd.Parameters.AddWithValue("@PROD_NOMBRE", obj.PROD_NOMBRE);
                    cmd.Parameters.AddWithValue("@PROD_MAR_ID", obj.oMarca.MAR_ID);
                    cmd.Parameters.AddWithValue("@PROD_CAT_ID", obj.oCategoria.CAT_ID);
                    cmd.Parameters.AddWithValue("@PROD_PRECIO", obj.PROD_PRECIO);
                    cmd.Parameters.AddWithValue("@PROD_STOCK", obj.PROD_STOCK);
                    cmd.Parameters.AddWithValue("@PROD_ACTIVO", obj.PROD_ACTIVO);
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

        public bool Editar(Producto obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_PRODUCTO", conex);

                    cmd.Parameters.AddWithValue("@PROD_ID", obj.PROD_ID);
                    cmd.Parameters.AddWithValue("@PROD_DESCRIPCION", obj.PROD_DESCRIPCION);
                    cmd.Parameters.AddWithValue("@PROD_NOMBRE", obj.PROD_NOMBRE);
                    cmd.Parameters.AddWithValue("@PROD_MAR_ID", obj.oMarca.MAR_ID);
                    cmd.Parameters.AddWithValue("@PROD_CAT_ID", obj.oCategoria.CAT_ID);
                    cmd.Parameters.AddWithValue("@PROD_PRECIO", obj.PROD_PRECIO);
                    cmd.Parameters.AddWithValue("@PROD_STOCK", obj.PROD_STOCK);
                    cmd.Parameters.AddWithValue("@PROD_ACTIVO", obj.PROD_ACTIVO);
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

        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    string query = "UPDATE PRODUCTO SET PROD_RUTAIMAGEN = @RUTAIMAGEN, PROD_NOMBREIMAGEN = @NOMBREIMAGEN WHERE PROD_ID = @PROD_ID";
                    SqlCommand cmd = new SqlCommand(query, conex);

                    cmd.Parameters.AddWithValue("@RUTAIMAGEN", obj.PROD_IMAGEN);
                    cmd.Parameters.AddWithValue("@NOMBREIMAGEN", obj.PROD_NOMBREIMAGEN);
                    cmd.Parameters.AddWithValue("@PROD_ID", obj.PROD_ID);
                    cmd.Parameters.AddWithValue("@RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;// significa que esta variable se llenara en la BD, no es parametro de entrada
                    cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    //cmd.CommandType = CommandType.StoredProcedure;

                    conex.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "NO SE PUDO ACTUALIZAR IMAGEN";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
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
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_PRODUCTO", conex);

                    cmd.Parameters.AddWithValue("@PROD_ID", id);
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

    }
}
