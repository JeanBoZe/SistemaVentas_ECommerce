using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // ============================================
        // 1. VISTAS (GET)
        // ============================================
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

        // ============================================
        // 2. LOGIN HÍBRIDO (CLIENTE O ADMIN)
        // ============================================
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente oCliente = null;
            Usuario oUsuario = null; // Variable para el admin

            // A. Intentamos buscar en la tabla CLIENTES
            oCliente = new CN_Cliente().Listar().Where(item => item.CLI_CORREO == correo && item.CLI_CLAVE == CN_Recursos.Encriptar(clave)).FirstOrDefault();

            if (oCliente != null)
            {
                // -- ES CLIENTE --
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
            else
            {
                // B. Si no es cliente, intentamos buscar en la tabla USUARIOS (ADMIN)
                oUsuario = new CN_Usuarios().Listar().Where(item => item.USU_CORREO == correo && item.USU_CLAVE == CN_Recursos.Encriptar(clave)).FirstOrDefault();

                if (oUsuario != null)
                {
                    // -- ES ADMINISTRADOR --
                    // Aquí no creamos sesión de Tienda, sino que redirigimos al PROYECTO ADMIN.
                    // ¡IMPORTANTE! Verifica que este puerto coincida con tu proyecto Admin.
                    return Redirect("https://localhost:44327/Home/Index");
                }
                else
                {
                    // C. NO EXISTE EN NINGUNO
                    ViewBag.Error = "Correo o contraseña no son correctos";
                    return View();
                }
            }
        }

        // ============================================
        // 3. REGISTRO DE CLIENTES
        // ============================================
        [HttpPost]
        public ActionResult Registrar(Cliente oCliente)
        {
            // Validamos contraseña de confirmación
            if (oCliente.CLI_CLAVE != oCliente.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View(oCliente);
            }

            string mensaje = string.Empty;

            // Encriptamos antes de enviar a la BD
            oCliente.CLI_CLAVE = CN_Recursos.Encriptar(oCliente.CLI_CLAVE);

            // Registramos
            int resultado = new CN_Cliente().Registrar(oCliente, out mensaje);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View(oCliente);
            }
        }

        // ============================================
        // 4. RESTABLECER Y CAMBIAR CLAVE
        // ============================================
        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Cliente oCliente = new CN_Cliente().Listar().Where(item => item.CLI_CORREO == correo).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "No se encontró un cliente con ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().ReestablecerClave(oCliente.CLI_ID, correo, out mensaje);

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
            int id = int.Parse(idcliente);
            string mensaje = string.Empty;

            Cliente oCliente = new CN_Cliente().Listar().Where(c => c.CLI_ID == id).FirstOrDefault();

            if (oCliente.CLI_CLAVE != CN_Recursos.Encriptar(claveactual))
            {
                TempData["IdCliente"] = idcliente;
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["IdCliente"] = idcliente;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            bool respuesta = new CN_Cliente().CambiarClave(id, CN_Recursos.Encriptar(nuevaclave), out mensaje);

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

        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> oLista = new List<Departamento>();
            oLista = new CN_Ubicacion().ObtenerDepartamento();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerProvincia(string IdDepartamento)
        {
            List<Provincia> oLista = new List<Provincia>();
            oLista = new CN_Ubicacion().ObtenerProvincia(IdDepartamento);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerDistrito(string IdDepartamento, string IdProvincia)
        {
            List<Distrito> oLista = new List<Distrito>();
            oLista = new CN_Ubicacion().ObtenerDistrito(IdDepartamento, IdProvincia);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
    }
}