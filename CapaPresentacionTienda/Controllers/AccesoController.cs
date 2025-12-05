using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {

        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            // 🔍 Verificar CLIENTE
            Cliente oCliente = new CN_Cliente().Listar()
                .FirstOrDefault(item => item.CLI_CORREO == correo && item.CLI_CLAVE == CN_Recursos.Encriptar(clave));

            if (oCliente != null)
            {
                if (oCliente.CLI_RESTABLECER)
                {
                    TempData["IdCliente"] = oCliente.CLI_ID;
                    return RedirectToAction("CambiarClave", "Acceso");
                }

                FormsAuthentication.SetAuthCookie(oCliente.CLI_CORREO, false);
                Session["Cliente"] = oCliente;
                ViewBag.Error = null;
                return RedirectToAction("Index", "Tienda");
            }

            // 🔍 Verificar USUARIO (admin)
            Usuario oUsuario = new CN_Usuarios().Listar()
                .FirstOrDefault(u => u.USU_CORREO == correo && u.USU_CLAVE == CN_Recursos.Encriptar(clave));

            if (oUsuario != null)
            {
                if (oUsuario.USU_RESTABLECER)
                {
                    TempData["IdUsuario"] = oUsuario.USU_ID;
                    return RedirectToAction("CambiarClave", "Acceso");
                }

                FormsAuthentication.SetAuthCookie(oUsuario.USU_CORREO, false);
                Session["Usuario"] = oUsuario;

                // 🔁 Redirige al proyecto Admin (cambia el puerto si es necesario)
                return Redirect("https://localhost:44327/");
            }

            // ❌ Si no existe en ninguna tabla
            ViewBag.Error = "Correo o contraseña no correcta";
            return View();
        }


        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Cliente ocliente = new Cliente();

            ocliente = new CN_Cliente().Listar().Where(item => item.CLI_CORREO == correo).FirstOrDefault();

            if (ocliente == null)
            {
                ViewBag.Error = "No se encontro un usuario relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().ReestablecerClave(ocliente.CLI_ID, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idcliente, string claveactual, string nuevaclave, string confirmarclave)
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(u => u.CLI_ID == int.Parse(idcliente)).FirstOrDefault();

            if (oCliente.CLI_CLAVE != CN_Recursos.Encriptar(claveactual))
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vClave"] = "";

                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vClave"] = claveactual;

                ViewBag.Error = "Las contrseñas no coinciden";
                return View();
            }

            ViewData["vClave"] = "";

            nuevaclave = CN_Recursos.Encriptar(nuevaclave);

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idcliente), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = idcliente;
                ViewBag.Error = mensaje;
                return View();
            }

        }

        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}