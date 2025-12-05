using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Comentario
    {

        private CD_Comentario objCapaDato = new CD_Comentario();

        public List<Comentario> Listar(int idProducto)
        {
            return objCapaDato.Listar(idProducto);
        }

        public bool Registrar(Comentario obj, out string Mensaje)
        {
            return objCapaDato.Registrar(obj, out Mensaje);
        }

    }
}
