using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class DetalleVenta
    {
        public int DETV_ID { get; set; }
        public int DETV_VEN_ID { get; set; }
        public Producto DETV_PROD_ID { get; set; }
        public int DETV_CANTIDAD { get; set; }
        public decimal DETV_TOTAL { get; set; }
        public int DETV_TRANS_ID { get; set; }
    }
}
