using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria obj_CapaDato = new CD_Categoria();

        public List<Categoria> Listar()
        {
            return obj_CapaDato.Listar();
        }

        public int Registrar(Categoria obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.CAT_DESCRIPCION) || string.IsNullOrWhiteSpace(obj.CAT_DESCRIPCION))
            {
                mensaje = "La descripcion de la categoria no puede ser vacio";
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

        public bool Editar(Categoria obj, out string mensaje)
        {

            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.CAT_DESCRIPCION) || string.IsNullOrWhiteSpace(obj.CAT_DESCRIPCION))
            {
                mensaje = "La descripcion de la categoria no puede ser vacio";
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

        public bool Eliminar(int id, out string Mensaje)
        {
            return obj_CapaDato.Eliminar(id, out Mensaje);
        }

    }
}
