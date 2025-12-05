using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Practica1_WebAvanzadas
{
    public partial class Cliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conBD"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_MOSTRAR_CLIENTE", conn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OP", 1);
                        //cmd.Parameters.AddWithValue("@CLI_ID", id); 
                        //cmd.Parameters.AddWithValue("@CLI_NOMBRE", nombre);
                        //cmd.Parameters.AddWithValue("@CLI_APELLIDOS", apellidos);
                        //cmd.Parameters.AddWithValue("@CLI_CELULAR", celular);
                        //cmd.Parameters.AddWithValue("@CLI_DIRECCION", direccion);
                        //cmd.Parameters.AddWithValue("@CLI_FECHANA", fechaNacimiento);
                        //cmd.Parameters.AddWithValue("@CLI_TIPOPAGO", tipoPago);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        gv_clientes.DataSource = dt;
                        gv_clientes.DataBind();
                    }
                }
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            string id = txt_ID.Text;
            string nombre = txt_nombre.Text;
            string apellidos = txt_apellidos.Text;
            string direccion = txt_direccion.Text;
            string telefono = txt_telefono.Text;
            string tipoPago = ddl_puesto.SelectedValue;
            string fechana = txt_fechana.Text;

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellidos) || string.IsNullOrWhiteSpace(direccion) ||
                string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(tipoPago) || string.IsNullOrWhiteSpace(fechana))
            {
                Response.Write("<script>alert('Todos los campos son obligatorios');</script>");
                return;
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conBD"].ConnectionString))
            {
                try
                {
                    
                    using (SqlCommand cmd = new SqlCommand("SP_MOSTRAR_CLIENTE", conn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OP", 2);
                        cmd.Parameters.AddWithValue("@CLI_ID", id);
                        cmd.Parameters.AddWithValue("@CLI_NOMBRE", nombre);
                        cmd.Parameters.AddWithValue("@CLI_APELLIDOS", apellidos);
                        cmd.Parameters.AddWithValue("@CLI_CELULAR", telefono);
                        cmd.Parameters.AddWithValue("@CLI_DIRECCION", direccion);
                        cmd.Parameters.AddWithValue("@CLI_FECHANA", fechana);
                        cmd.Parameters.AddWithValue("@CLI_TIPOPAGO", tipoPago);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    Response.Write("<script>alert('Cliente guardado correctamente');</script>");
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error al guardar: " + ex.Message + "');</script>");
                }
                CargarGV();
            }
        }

        public void CargarGV()
        {
             using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conBD"].ConnectionString))
             {
                 using (SqlCommand cmd = new SqlCommand("SP_MOSTRAR_CLIENTE", conn))
                 {

                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.AddWithValue("@OP", 1);
                     //cmd.Parameters.AddWithValue("@CLI_ID", id); 
                     //cmd.Parameters.AddWithValue("@CLI_NOMBRE", nombre);
                     //cmd.Parameters.AddWithValue("@CLI_APELLIDOS", apellidos);
                     //cmd.Parameters.AddWithValue("@CLI_CELULAR", celular);
                     //cmd.Parameters.AddWithValue("@CLI_DIRECCION", direccion);
                     //cmd.Parameters.AddWithValue("@CLI_FECHANA", fechaNacimiento);
                     //cmd.Parameters.AddWithValue("@CLI_TIPOPAGO", tipoPago);
                     SqlDataAdapter da = new SqlDataAdapter(cmd);
                     DataTable dt = new DataTable();
                     da.Fill(dt);

                     gv_clientes.DataSource = dt;
                     gv_clientes.DataBind();
                 }
             }
            
        }

        protected void gv_clientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gv_clientes.Rows[index];

                txt_ID.Text = row.Cells[0].Text;
                txt_nombre.Text = row.Cells[1].Text;
                txt_apellidos.Text = row.Cells[2].Text;
                txt_telefono.Text = row.Cells[3].Text;
                txt_direccion.Text = row.Cells[4].Text;
                txt_fechana.Text = row.Cells[5].Text;
                ddl_puesto.SelectedIndex = int.Parse(row.Cells[6].Text);

                //if (row.Cells[6].Text == "Tarjeta de Debito")
                //    ddl_puesto.SelectedIndex = 1;
                //else if(row.Cells[6].Text == "Tarjeta de Credito")
                //    ddl_puesto.SelectedIndex = 2;
                //else if (row.Cells[6].Text == "Deposito")
                //    ddl_puesto.SelectedIndex = 3;
            }

        }

        protected void btnBorrar_Click(object sender, EventArgs e)
        {
            string id = txt_ID.Text;


            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conBD"].ConnectionString))
            {
                try
                {

                    using (SqlCommand cmd = new SqlCommand("SP_MOSTRAR_CLIENTE", conn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OP", 3);
                        cmd.Parameters.AddWithValue("@CLI_ID", id);


                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    Response.Write("<script>alert('Cliente borrado correctamente');</script>");
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error al borrar: " + ex.Message + "');</script>");
                }
                CargarGV();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

        }
    }
}