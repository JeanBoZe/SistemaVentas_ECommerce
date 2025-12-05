using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Producto
    {

        private CD_Producto obj_CapaDato = new CD_Producto();

        public List<Producto> Listar()
        {
            return obj_CapaDato.Listar();
        }

        public int Registrar(Producto obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.PROD_NOMBRE) || string.IsNullOrWhiteSpace(obj.PROD_NOMBRE))
            {
                mensaje = "El nombre del producto no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.PROD_DESCRIPCION) || string.IsNullOrWhiteSpace(obj.PROD_DESCRIPCION))
            {
                mensaje = "La descripcion del producto no puede ser vacio";
            }
            else if(obj.oMarca.MAR_ID == 0)
            {
                mensaje = "Debe seleccionar una marca";
            }
            else if(obj.oCategoria.CAT_ID == 0)
            {
                mensaje = "Debe seleccionar una categoria";
            }
            else if(obj.PROD_PRECIO == 0)
            {
                mensaje = "Debe ingresar el precio del producto";
            }
            else if (obj.PROD_STOCK == 0)
            {
                mensaje = "Debe ingresar el stock del producto";
            }

            if (string.IsNullOrEmpty(mensaje))
            {

                return obj_CapaDato.Registrar(obj, out mensaje);

            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Producto obj, out string mensaje)
        {

            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.PROD_NOMBRE) || string.IsNullOrWhiteSpace(obj.PROD_NOMBRE))
            {
                mensaje = "El nombre del producto no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.PROD_DESCRIPCION) || string.IsNullOrWhiteSpace(obj.PROD_DESCRIPCION))
            {
                mensaje = "La descripcion del producto no puede ser vacio";
            }
            else if (obj.oMarca.MAR_ID == 0)
            {
                mensaje = "Debe seleccionar una marca";
            }
            else if (obj.oCategoria.CAT_ID == 0)
            {
                mensaje = "Debe seleccionar una categoria";
            }
            else if (obj.PROD_PRECIO == 0)
            {
                mensaje = "Debe ingresar el precio del producto";
            }
            else if (obj.PROD_STOCK == 0)
            {
                mensaje = "Debe ingresar el stock del producto";
            }

            if (string.IsNullOrEmpty(mensaje))
            {

                return obj_CapaDato.Editar(obj, out mensaje);

            }
            else
            {
                return false;
            }
        }

        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            return obj_CapaDato.GuardarDatosImagen(obj, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return obj_CapaDato.Eliminar(id, out Mensaje);
        }


    }
}
