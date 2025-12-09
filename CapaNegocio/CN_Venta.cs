using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Venta
    {

        private CD_Venta obj_CapaDato = new CD_Venta();

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string mensaje)
        {
            return obj_CapaDato.Registrar(obj, DetalleVenta, out mensaje);
        }

        public bool VerificarCompra(int idCliente, int idProducto)
        {
            return new CD_Venta().VerificarCompra(idCliente, idProducto);
        }
    }
}
