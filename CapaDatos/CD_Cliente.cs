using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Cliente
    {

        public int Registrar(Cliente obj, out string mensaje)
        {
            int idAutugenerado = 0;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRAR_CLIENTE", conex);

                    cmd.Parameters.AddWithValue("@CLI_NOMBRES", obj.CLI_NOMBRE);
                    cmd.Parameters.AddWithValue("@CLI_APELLIDOS", obj.CLI_APELLIDOS);
                    cmd.Parameters.AddWithValue("@CLI_CORREO", obj.CLI_CORREO);
                    cmd.Parameters.AddWithValue("@CLI_CLAVE", obj.CLI_CLAVE);
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


        public List<Cliente> Listar()
        {

            List<Cliente> lista = new List<Cliente>();
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT CLI_ID, CLI_NOMBRES, CLI_APELLIDOS, CLI_CORREO, CLI_CLAVE, CLI_RESTABLECER FROM CLIENTE ";
                    SqlCommand cmd = new SqlCommand(query, conex);
                    cmd.CommandType = CommandType.Text;

                    conex.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Cliente()
                                {
                                    CLI_ID = Convert.ToInt32(dr["CLI_ID"]),
                                    CLI_NOMBRE = dr["CLI_NOMBRES"].ToString(),
                                    CLI_APELLIDOS = dr["CLI_APELLIDOS"].ToString(),
                                    CLI_CORREO = dr["CLI_CORREO"].ToString(),
                                    CLI_CLAVE = dr["CLI_CLAVE"].ToString(),
                                    CLI_RESTABLECER = Convert.ToBoolean(dr["CLI_RESTABLECER"]),
                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Cliente>();
            }

            return lista;

        }

        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE CLIENTE SET CLI_CLAVE = @NUEVACLAVE, CLI_RESTABLECER = 0 WHERE CLI_ID = @ID", conex);
                    cmd.Parameters.AddWithValue("@ID", idcliente);
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

        public bool ReestablecerClave(int idcliente, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE CLIENTE SET CLI_CLAVE = @CLAVE, CLI_RESTABLECER = 1 WHERE CLI_ID = @ID;", conex);
                    cmd.Parameters.AddWithValue("@ID", idcliente);
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
