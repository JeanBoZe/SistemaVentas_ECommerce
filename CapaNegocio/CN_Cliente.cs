using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {

        private CD_Cliente obj_CapaDato = new CD_Cliente();
        
        public int Registrar(Cliente obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.CLI_NOMBRE) || string.IsNullOrWhiteSpace(obj.CLI_NOMBRE))
            {
                mensaje = "El nombre del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.CLI_APELLIDOS) || string.IsNullOrWhiteSpace(obj.CLI_APELLIDOS))
            {
                mensaje = "El apellido del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.CLI_CORREO) || string.IsNullOrWhiteSpace(obj.CLI_CORREO))
            {
                mensaje = "El correo del cliente no puede ser vacio";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                obj.CLI_CLAVE = CN_Recursos.Encriptar(obj.CLI_CLAVE);
                return obj_CapaDato.Registrar(obj, out mensaje);

            }
            else
            {
                return 0;
            }
        }

        public List<Cliente> Listar()
        {
            return obj_CapaDato.Listar();
        }

        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {
            return obj_CapaDato.CambiarClave(idcliente, nuevaclave, out Mensaje);
        }

        public bool ReestablecerClave(int idcliente, string correo, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = obj_CapaDato.ReestablecerClave(idcliente, CN_Recursos.Encriptar(nuevaclave), out mensaje);

            if (resultado)
            {
                string asunto = "CONSTRASEÑA REESTABLECIDA";
                string mensajeCorreo = "<h3>Cuenta reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", nuevaclave);

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensajeCorreo);

                if (respuesta == true)
                {
                    return true;


                }
                else
                {
                    mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }
        }


    }
}
