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
    public class CD_Venta
    {

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string mensaje)
        {
            bool respuesta = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarVenta", conex);

                    cmd.Parameters.AddWithValue("@IdCliente", obj.VEN_CLI_ID);
                    cmd.Parameters.AddWithValue("@TotalProducto", obj.VEN_TOTALPRODUCTO);
                    cmd.Parameters.AddWithValue("@MontoTotal", obj.VEN_MONTOTOTAL);
                    cmd.Parameters.AddWithValue("@Contacto", obj.VEN_CONTACTO);
                    cmd.Parameters.AddWithValue("@IdDistrito", obj.VEN_DIS_ID);
                    cmd.Parameters.AddWithValue("@Telefono", obj.VEN_TELEFONO);
                    cmd.Parameters.AddWithValue("@Direccion", obj.VEN_DIRECCION);
                    cmd.Parameters.AddWithValue("@IdTransaccion", obj.VEN_TRANS_ID);
                    cmd.Parameters.AddWithValue("@DetalleVenta", DetalleVenta);
                    cmd.Parameters.AddWithValue("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;// significa que esta variable se llenara en la BD, no es parametro de entrada
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conex.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                mensaje = ex.Message;
            }
            return respuesta;
        }

    }
}
