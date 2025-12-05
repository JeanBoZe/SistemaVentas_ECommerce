using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Producto
    {
        public int PROD_ID { get; set; }
        public string PROD_NOMBRE { get; set; }
        public string PROD_DESCRIPCION { get; set; }
        public Marca oMarca { get; set; }
        public Categoria oCategoria { get; set; }
        public decimal PROD_PRECIO { get; set; }
        public string PrecioTexto { get; set; }
        public int PROD_STOCK { get; set; }
        public string PROD_IMAGEN { get; set; }
        public string PROD_NOMBREIMAGEN { get; set; }
        public bool PROD_ACTIVO { get; set; }
        public string Base64 { get; set; }
        public string Extension { get; set; }
        public List<Comentario> oListaComentarios { get; set; }
    }
}
