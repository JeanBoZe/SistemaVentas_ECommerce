using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Comentario
    {
        public int COM_ID { get; set; }
        public int COM_PROD_ID { get; set; }
        public int COM_CLI_ID { get; set; }
        public int COM_PUNTUACION { get; set; }
        public string COM_CONTENIDO { get; set; }
        public string COM_FECHAREGISTRO { get; set; }

        // --- PROPIEDADES ADICIONALES ---
        // Estas no se guardan en la tabla COMENTARIO, 
        // pero sirven para "cachcachar" el nombre cuando haces el JOIN con CLIENTE.
        public string NombreCliente { get; set; }
    }
}
