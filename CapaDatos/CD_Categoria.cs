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
    public class CD_Categoria
    {
        public List<Categoria> Listar()
        {

            List<Categoria> lista = new List<Categoria>();
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT CAT_ID, CAT_DESCRIPCION, CAT_ACTIVO FROM CATEGORIA";
                    SqlCommand cmd = new SqlCommand(query, conex);
                    cmd.CommandType = CommandType.Text;

                    conex.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Categoria()
                                {
                                    CAT_ID = Convert.ToInt32(dr["CAT_ID"]),
                                    CAT_DESCRIPCION = dr["CAT_DESCRIPCION"].ToString(),
                                    CAT_ACTIVO = Convert.ToBoolean(dr["CAT_ACTIVO"])
                                });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Categoria>();
            }

            return lista;

        }

        public int Registrar(Categoria obj, out string mensaje)
        {
            int idAutugenerado = 0;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRAR_CATEGORIA", conex);

                    cmd.Parameters.AddWithValue("@DESCRIPCION", obj.CAT_DESCRIPCION);
                    cmd.Parameters.AddWithValue("@ACTIVO", obj.CAT_ACTIVO);
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

        public bool Editar(Categoria obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_CATEGORIA", conex);

                    cmd.Parameters.AddWithValue("@CAT_ID", obj.CAT_ID);
                    cmd.Parameters.AddWithValue("@CAT_DESCRIPCION", obj.CAT_DESCRIPCION);
                    cmd.Parameters.AddWithValue("@CAT_ACTIVO", obj.CAT_ACTIVO);
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
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_CATEGORIA", conex);

                    cmd.Parameters.AddWithValue("@CAT_ID", id);
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
