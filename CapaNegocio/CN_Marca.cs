using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Marca
    {

        private CD_Marca obj_CapaDato = new CD_Marca();

        public List<Marca> Listar()
        {
            return obj_CapaDato.Listar();
        }

        public int Registrar(Marca obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.MAR_DESCRIPCION) || string.IsNullOrWhiteSpace(obj.MAR_DESCRIPCION))
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

        public bool Editar(Marca obj, out string mensaje)
        {

            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.MAR_DESCRIPCION) || string.IsNullOrWhiteSpace(obj.MAR_DESCRIPCION))
            {
                mensaje = "La descripcion de la marca no puede ser vacio";
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

        public List<Marca> ListarMarcaPorCategoria(int idcategoria)
        {
            return obj_CapaDato.ListarMarcaPorCategoria(idcategoria);
        }
    }
}
