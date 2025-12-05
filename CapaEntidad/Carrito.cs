using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Carrito
    {
        public int CARRITO_ID { get; set; }
        public Cliente CARRITO_CLI_ID { get; set; }
        public Producto oProducto { get; set; }
        public int CARRITO_CANTIDAD { get; set; }
    }
}
