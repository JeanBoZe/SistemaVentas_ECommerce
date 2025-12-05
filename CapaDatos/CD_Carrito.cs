using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Carrito
    {

        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EXISTECARRITO", conex);

                    cmd.Parameters.AddWithValue("@CLI_ID", idcliente);
                    cmd.Parameters.AddWithValue("@PROD_ID", idproducto);
                    cmd.Parameters.AddWithValue("@RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;// significa que esta variable se llenara en la BD, no es parametro de entrada
                    
                    cmd.CommandType = CommandType.StoredProcedure;

                    conex.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@RESULTADO"].Value);

                }
            }
            catch (Exception ex)
            {
                resultado = false;

            }
            return resultado;
        }

        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_OPERACIONCARRITO", conex);

                    cmd.Parameters.AddWithValue("@IDCLIENTE", idcliente);
                    cmd.Parameters.AddWithValue("@IDPRODUCTO", idproducto);
                    cmd.Parameters.AddWithValue("@SUMAR", sumar);
                    cmd.Parameters.AddWithValue("@RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;// significa que esta variable se llenara en la BD, no es parametro de entrada
                    cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conex.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@RESULTADO"].Value);
                    Mensaje = cmd.Parameters["@MENSAJE"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        //

        public int CantidadEnCarrito(int idcliente)
        {
            int resultado = 0;

            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM CARRITO WHERE CARR_CLI_ID = @idcliente", conex);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;
                    conex.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                resultado = 0;

            }
            return resultado;
        }

        public List<Carrito> ListarProducto(int idcliente)
        {

            List<Carrito> lista = new List<Carrito>();

            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {

                    string query = "SELECT * FROM FN_OBTENERCARRITOCLIENTE(@idcliente)";

                    SqlCommand cmd = new SqlCommand(query, conex);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;

                    conex.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Carrito()
                            {
                                 oProducto = new Producto()
                                 {
                                     PROD_ID = Convert.ToInt32(dr["PROD_ID"]),
                                     PROD_NOMBRE = dr["PROD_NOMBRE"].ToString(),
                                     PROD_PRECIO = Convert.ToDecimal(dr["PROD_PRECIO"], new CultureInfo("es-MX")),
                                     PROD_IMAGEN = dr["PROD_RUTAIMAGEN"].ToString(),
                                     PROD_NOMBREIMAGEN = dr["PROD_NOMBREIMAGEN"].ToString(),
                                     //PROD_ACTIVO = Convert.ToBoolean(dr["PROD_ACTIVO"]),
                                     oMarca = new Marca() { MAR_DESCRIPCION = dr["DesMarca"].ToString()}
                                 },
                                 CARRITO_CANTIDAD = Convert.ToInt32(dr["CARR_CANTIAD"])
                            });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                lista = new List<Carrito>();
            }

            return lista;

        }


        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarCarrito", conex);

                    cmd.Parameters.AddWithValue("@IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("@IdProducto", idproducto);
                    cmd.Parameters.AddWithValue("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;// significa que esta variable se llenara en la BD, no es parametro de entrada

                    cmd.CommandType = CommandType.StoredProcedure;

                    conex.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);

                }
            }
            catch (Exception ex)
            {
                resultado = false;

            }
            return resultado;
        }

    }
}
