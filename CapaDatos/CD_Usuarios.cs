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
    public class CD_Usuarios
    {

        public List<Usuario> Listar()
        {

            List<Usuario> lista = new List<Usuario>();
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT U.USU_ID,U.USU_NOMBRES,U.USU_APELLIDOS,U.USU_CORREO,U.USU_CLAVE,U.USU_RESTABLECER,U.USU_ACTIVO FROM USUARIO U";
                    SqlCommand cmd = new SqlCommand(query, conex);
                    cmd.CommandType = CommandType.Text;

                    conex.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Usuario()
                                {
                                    USU_ID = Convert.ToInt32(dr["USU_ID"]),
                                    USU_NOMBRES = dr["USU_NOMBRES"].ToString(),
                                    USU_APELLIDOS = dr["USU_APELLIDOS"].ToString(),
                                    USU_CORREO = dr["USU_CORREO"].ToString(),
                                    USU_CLAVE = dr["USU_CLAVE"].ToString(),
                                    USU_RESTABLECER = Convert.ToBoolean(dr["USU_RESTABLECER"]),
                                    USU_ACTIVO = Convert.ToBoolean(dr["USU_ACTIVO"])
                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Usuario>();
            }

            return lista;

        }

        public int Registrar(Usuario obj, out string mensaje)
        {
            int idAutugenerado = 0;
            mensaje = string.Empty;
            try
            {
                using(SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARUSUARIO", conex);

                    cmd.Parameters.AddWithValue("@NOMBRES", obj.USU_NOMBRES);
                    cmd.Parameters.AddWithValue("@APELLIDOS", obj.USU_APELLIDOS);
                    cmd.Parameters.AddWithValue("@CORREO", obj.USU_CORREO);
                    cmd.Parameters.AddWithValue("@CLAVE", obj.USU_CLAVE);
                    cmd.Parameters.AddWithValue("@ACTIVO", obj.USU_ACTIVO);
                    cmd.Parameters.AddWithValue("@RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;// significa que esta variable se llenara en la BD, no es parametro de entrada
                    cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conex.Open();
                    cmd.ExecuteNonQuery();

                    idAutugenerado = Convert.ToInt32(cmd.Parameters["@RESULTADO"].Value);
                    mensaje = cmd.Parameters["@MENSAJE"].Value.ToString();
                }
            }
            catch(Exception ex)
            {
                idAutugenerado = 0;
                mensaje = ex.Message;
            }
            return idAutugenerado;
        }

        public bool Editar(Usuario obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITARUSUARIO", conex);

                    cmd.Parameters.AddWithValue("@USU_ID", obj.USU_ID);
                    cmd.Parameters.AddWithValue("@NOMBRES", obj.USU_NOMBRES);
                    cmd.Parameters.AddWithValue("@APELLIDOS", obj.USU_APELLIDOS);
                    cmd.Parameters.AddWithValue("@CORREO", obj.USU_CORREO);
                    cmd.Parameters.AddWithValue("@ACTIVO", obj.USU_ACTIVO);
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

        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("DELETE TOP (1) FROM USUARIO WHERE USU_ID = @ID", conex);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    conex.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;//condicion para saber si por lo menos una fila se afecto
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool CambiarClave(int idusuario, string nuevaclave,out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET USU_CLAVE = @NUEVACLAVE, USU_RESTABLECER = 0 WHERE USU_ID = @ID", conex);
                    cmd.Parameters.AddWithValue("@ID", idusuario);
                    cmd.Parameters.AddWithValue("@NUEVACLAVE", nuevaclave);
                    cmd.CommandType = CommandType.Text;
                    conex.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;//condicion para saber si por lo menos una fila se afecto
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool ReestablecerClave(int idusuario, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET USU_CLAVE = @CLAVE, USU_RESTABLECER = 1 WHERE USU_ID = @ID", conex);
                    cmd.Parameters.AddWithValue("@ID", idusuario);
                    cmd.Parameters.AddWithValue("@CLAVE", clave);
                    cmd.CommandType = CommandType.Text;
                    conex.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;//condicion para saber si por lo menos una fila se afecto
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
