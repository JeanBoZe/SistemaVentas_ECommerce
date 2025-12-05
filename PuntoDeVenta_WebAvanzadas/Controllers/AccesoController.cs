using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Web.Security;

namespace PuntoDeVenta_WebAvanzadas.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            // 🔹 Primero intenta iniciar sesión como CLIENTE
            Cliente oCliente = new CN_Cliente().Listar().FirstOrDefault(c =>
                c.CLI_CORREO == correo &&
                c.CLI_CLAVE == CN_Recursos.Encriptar(clave)
            );

            if (oCliente != null)
            {
                // 🔸 Redirige a la tienda (CapaPresentacionTienda)
                return Redirect("https://localhost:44358/Acceso/Index"); // ← reemplaza con el puerto correcto
            }

            // 🔹 Luego intenta iniciar sesión como USUARIO (admin)
            Usuario oUsuario = new CN_Usuarios().Listar().FirstOrDefault(u =>
                u.USU_CORREO == correo &&
                u.USU_CLAVE == CN_Recursos.Encriptar(clave)
            );

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña no correcta";
                return View();
            }
            else
            {
                if (oUsuario.USU_RESTABLECER) // Primera vez
                {
                    TempData["IdUsuario"] = oUsuario.USU_ID;
                    return RedirectToAction("CambiarClave");
                }

                FormsAuthentication.SetAuthCookie(oUsuario.USU_CORREO, false);
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.USU_ID == int.Parse(idusuario)).FirstOrDefault();

            if(oUsuario.USU_CLAVE != CN_Recursos.Encriptar(claveactual))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vClave"] = "";

                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if(nuevaclave!= confirmarclave)
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vClave"] = claveactual;

                ViewBag.Error = "Las contrseñas no coinciden";
                return View();
            }

            ViewData["vClave"] = "";

            nuevaclave = CN_Recursos.Encriptar(nuevaclave);

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else 
            {
                TempData["IdUsuario"] = idusuario;
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario ousuario = new Usuario();

            ousuario = new CN_Usuarios().Listar().Where(item => item.USU_CORREO == correo).FirstOrDefault();

            if(ousuario == null)
            {
                ViewBag.Error = "No se encontro un usuario relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().ReestablecerClave(ousuario.USU_ID, correo, out mensaje);

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

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }

    }
}