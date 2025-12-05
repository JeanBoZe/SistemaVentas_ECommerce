using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {

        private CD_Usuarios obj_CapaDato = new CD_Usuarios();
        public List<Usuario> Listar()
        {
            return obj_CapaDato.Listar();
        }

        public int Registrar(Usuario obj, out string mensaje)
        {
            mensaje = string.Empty;

            if(string.IsNullOrEmpty(obj.USU_NOMBRES) || string.IsNullOrWhiteSpace(obj.USU_NOMBRES))
            {
                mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.USU_APELLIDOS) || string.IsNullOrWhiteSpace(obj.USU_APELLIDOS))
            {
                mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.USU_CORREO) || string.IsNullOrWhiteSpace(obj.USU_CORREO))
            {
                mensaje = "El correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                string clave = CN_Recursos.GenerarClave();
                string asunto = "CLAVE PARA TU CUENTA";
                string mensajeCorreo = "<h3>Cuenta creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", clave);

                bool respuesta = CN_Recursos.EnviarCorreo(obj.USU_CORREO, asunto, mensajeCorreo);
                if (respuesta == true)
                {
                    obj.USU_CLAVE = CN_Recursos.Encriptar(clave);//mandamos llamar al metodo para encriptar clave

                    return obj_CapaDato.Registrar(obj, out mensaje);
                }
                else
                {
                    mensaje = "No se pudo enviar el correo";
                    return 0;
                } 
            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Usuario obj, out string mensaje)
        {

            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.USU_NOMBRES) || string.IsNullOrWhiteSpace(obj.USU_NOMBRES))
            {
                mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.USU_APELLIDOS) || string.IsNullOrWhiteSpace(obj.USU_APELLIDOS))
            {
                mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.USU_CORREO) || string.IsNullOrWhiteSpace(obj.USU_CORREO))
            {
                mensaje = "El correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(mensaje))
            {

                return obj_CapaDato.Editar(obj, out mensaje);

            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return obj_CapaDato.Eliminar(id, out Mensaje);
        }

        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            return obj_CapaDato.CambiarClave(idusuario,nuevaclave, out Mensaje);
        }

        public bool ReestablecerClave(int idusuario, string correo, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = obj_CapaDato.ReestablecerClave(idusuario,CN_Recursos.Encriptar(nuevaclave), out mensaje);

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
