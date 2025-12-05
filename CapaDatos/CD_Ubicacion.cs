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
    public class CD_Ubicacion
    {

        public List<Departamento> ObtenerDepartamento()
        {

            List<Departamento> lista = new List<Departamento>();
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM DEPARTAMENTO";
                    SqlCommand cmd = new SqlCommand(query, conex);
                    cmd.CommandType = CommandType.Text;

                    conex.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Departamento()
                                {
                                    DEP_ID = dr["DEP_ID"].ToString(),
                                    DEP_DESCRIPCION = dr["DEP_DESCRIPCION"].ToString(),
                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Departamento>();
            }

            return lista;

        }

        public List<Provincia> ObtenerProvincia(string iddepartamento)
        {

            List<Provincia> lista = new List<Provincia>();
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM PROVINCIA WHERE PROVI_DEP_ID = @iddepartamento";
                    SqlCommand cmd = new SqlCommand(query, conex);
                    cmd.Parameters.AddWithValue("@iddepartamento", iddepartamento);
                    cmd.CommandType = CommandType.Text;

                    conex.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Provincia()
                                {
                                    PROVI_ID = dr["PROVI_ID"].ToString(),
                                    PROVI_DESCRIPCION = dr["PROVI_DESCRIPCION"].ToString(),
                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Provincia>();
            }

            return lista;

        }

        public List<Distrito> ObtenerDistrito(string iddepartamento, string idprovincia)
        {

            List<Distrito> lista = new List<Distrito>();
            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM DISTRITO WHERE DIS_PROV_ID = @idprovincia AND DIS_DEP_ID = @iddepartamento";
                    SqlCommand cmd = new SqlCommand(query, conex);
                    cmd.Parameters.AddWithValue("@idprovincia", idprovincia);
                    cmd.Parameters.AddWithValue("@iddepartamento", iddepartamento);

                    cmd.CommandType = CommandType.Text;

                    conex.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Distrito()
                                {
                                    DIST_ID = dr["DIS_ID"].ToString(),
                                    DIST_DESCRIPCION = dr["DIS_DESCRIPCION"].ToString(),
                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Distrito>();
            }

            return lista;

        }

    }
}
