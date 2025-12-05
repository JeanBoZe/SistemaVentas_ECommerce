using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Reporte
    {

        private CD_Reporte obj_CapaDato = new CD_Reporte();

        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            return obj_CapaDato.Ventas(fechainicio, fechafin, idtransaccion);
        }

        public DashBoard VerDashBoard()
        {
            return obj_CapaDato.VerDashBoard();
        }


    }
}
