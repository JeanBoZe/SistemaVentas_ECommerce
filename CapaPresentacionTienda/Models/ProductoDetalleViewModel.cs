using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CapaEntidad;

namespace CapaPresentacionTienda.Models
{
    public class ProductoDetalleViewModel
    {
        // El producto que ya tenías
        public Producto oProducto { get; set; }

        // La nueva lista de comentarios
        public List<Comentario> oListaComentarios { get; set; }
    }
}