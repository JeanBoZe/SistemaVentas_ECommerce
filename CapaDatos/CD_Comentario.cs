using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CapaEntidad;
using System.Data;

namespace CapaDatos
{
    public class CD_Comentario
    {

        public List<Comentario> Listar(int idProducto)
        {
            List<Comentario> lista = new List<Comentario>();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "sp_ListarComentarios";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Comentario()
                            {
                                COM_ID = Convert.ToInt32(dr["COM_ID"]),
                                COM_PUNTUACION = Convert.ToInt32(dr["COM_PUNTUACION"]),
                                COM_CONTENIDO = dr["COM_CONTENIDO"].ToString(),
                                COM_FECHAREGISTRO = dr["COM_FECHAREGISTRO"].ToString(),
                                NombreCliente = dr["CLI_NOMBRES"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Comentario>();
            }
            return lista;
        }

        public bool Registrar(Comentario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarComentario", oconexion);
                    cmd.Parameters.AddWithValue("@IdProducto", obj.COM_PROD_ID);
                    cmd.Parameters.AddWithValue("@IdCliente", obj.COM_CLI_ID);
                    cmd.Parameters.AddWithValue("@Puntuacion", obj.COM_PUNTUACION);
                    cmd.Parameters.AddWithValue("@Contenido", obj.COM_CONTENIDO);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

    }
}
