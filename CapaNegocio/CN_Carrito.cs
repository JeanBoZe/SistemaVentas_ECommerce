using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Carrito
    {

        private CD_Carrito obj_CapaDato = new CD_Carrito();

        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            return obj_CapaDato.ExisteCarrito(idcliente, idproducto);
        }

        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            return obj_CapaDato.OperacionCarrito(idcliente, idproducto, sumar, out Mensaje);
        }

        public int CantidadEnCarrito(int idcliente)
        {
            return obj_CapaDato.CantidadEnCarrito(idcliente);
        }

        public List<Carrito> ListarProducto(int idcliente)
        {
            return obj_CapaDato.ListarProducto(idcliente);
        }

        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            return obj_CapaDato.EliminarCarrito(idcliente, idproducto);
        }
    }
}
