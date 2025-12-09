using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // ============================================
        // 1. VISTAS PRINCIPALES
        // ============================================
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Carrito()
        {
            return View();
        }

        public ActionResult DetalleProducto(int idproducto = 0)
        {
            if (idproducto == 0) return RedirectToAction("Index");

            Producto oProducto = new CN_Producto().Listar().Where(p => p.PROD_ID == idproducto).FirstOrDefault();

            if (oProducto != null)
            {
                // Cargar Comentarios
                oProducto.oListaComentarios = new CN_Comentario().Listar(idproducto);

                // Cargar Imagen: Priorizamos la ruta que viene de la BD
                oProducto.Base64 = ConvertirImagenABase64(oProducto.PROD_IMAGEN, oProducto.PROD_NOMBREIMAGEN);
                oProducto.Extension = Path.GetExtension(oProducto.PROD_NOMBREIMAGEN).Replace(".", "");
            }

            return View(oProducto);
        }

        // ============================================
        // 2. CATÁLOGO Y FILTROS
        // ============================================
        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            List<Producto> lista = new CN_Producto().Listar();

            if (idcategoria > 0)
                lista = lista.Where(p => p.oCategoria.CAT_ID == idcategoria).ToList();

            if (idmarca > 0)
                lista = lista.Where(p => p.oMarca.MAR_ID == idmarca).ToList();

            foreach (Producto item in lista)
            {
                // Pasamos la ruta de la BD (PROD_IMAGEN) para que la use si existe
                item.Base64 = ConvertirImagenABase64(item.PROD_IMAGEN, item.PROD_NOMBREIMAGEN);
                item.Extension = Path.GetExtension(item.PROD_NOMBREIMAGEN).Replace(".", "");
            }

            var jsonResult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new CN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListaMarcasPorCategoria(int idcategoria)
        {
            List<Marca> lista = new CN_Marca().ListarMarcaPorCategoria(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        // ============================================
        // 3. CARRITO DE COMPRAS (Corregido)
        // ============================================
        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            if (Session["Cliente"] == null)
                return Json(new { respuesta = false, mensaje = "Inicia sesión para comprar" });

            int idcliente = ((Cliente)Session["Cliente"]).CLI_ID;
            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);
            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = 0;
            if (Session["Cliente"] != null)
            {
                idcliente = ((Cliente)Session["Cliente"]).CLI_ID;
            }

            int cantidad = new CN_Carrito().CantidadEnCarrito(idcliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int idcliente = 0;
            if (Session["Cliente"] != null)
            {
                idcliente = ((Cliente)Session["Cliente"]).CLI_ID;
            }

            List<Carrito> oLista = new CN_Carrito().ListarProducto(idcliente);

            foreach (Carrito item in oLista)
            {
                // Convertir imágenes también en el carrito
                item.oProducto.Base64 = ConvertirImagenABase64(item.oProducto.PROD_IMAGEN, item.oProducto.PROD_NOMBREIMAGEN);
                item.oProducto.Extension = Path.GetExtension(item.oProducto.PROD_NOMBREIMAGEN).Replace(".", "");
            }

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            int idcliente = ((Cliente)Session["Cliente"]).CLI_ID;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, sumar, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).CLI_ID;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idcliente, idproducto);
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> oLista = new List<Departamento>();
            // Usamos la misma Capa de Negocio que ya tienes configurada
            oLista = new CN_Ubicacion().ObtenerDepartamento();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerProvincia(string IdDepartamento)
        {
            List<Provincia> oLista = new List<Provincia>();
            oLista = new CN_Ubicacion().ObtenerProvincia(IdDepartamento);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDistrito(string IdDepartamento, string IdProvincia)
        {
            List<Distrito> oLista = new List<Distrito>();
            oLista = new CN_Ubicacion().ObtenerDistrito(IdDepartamento, IdProvincia);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarComentario(string contenido, int puntuacion, int idproducto)
        {
            if (Session["Cliente"] == null) return Json(new { resultado = false, mensaje = "Debes iniciar sesión" });

            Cliente oCliente = (Cliente)Session["Cliente"];
            Comentario oComentario = new Comentario()
            {
                COM_PROD_ID = idproducto,
                COM_CLI_ID = oCliente.CLI_ID,
                COM_PUNTUACION = puntuacion,
                COM_CONTENIDO = contenido
            };
            string mensaje;
            bool resultado = new CN_Comentario().Registrar(oComentario, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        // ============================================
        // 6. HELPER DE IMÁGENES (Lógica Robusta)
        // ============================================
        private string ConvertirImagenABase64(string rutaBD, string nombreImagen)
        {
            string base64String = null;
            string rutaFinal = "";

            try
            {
                // 1. Intento A: Usar la ruta completa que viene de la BD (si existe)
                // Ej: "D:\Universidad\...\Images_Producto" + "\imagen.jpg"
                if (!string.IsNullOrEmpty(rutaBD))
                {
                    rutaFinal = Path.Combine(rutaBD, nombreImagen);
                }

                // 2. Intento B: Si la ruta de BD no sirve, calcularla manualmente relativo al proyecto
                if (string.IsNullOrEmpty(rutaFinal) || !System.IO.File.Exists(rutaFinal))
                {
                    string rutaTienda = Server.MapPath("~");
                    string rutaSolucion = Directory.GetParent(rutaTienda).FullName;
                    rutaFinal = Path.Combine(rutaSolucion, "Images_Producto", nombreImagen);
                }

                // 3. Leer y convertir
                if (System.IO.File.Exists(rutaFinal))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(rutaFinal);
                    base64String = Convert.ToBase64String(bytes);
                }
            }
            catch
            {
                base64String = null;
            }
            return base64String;
        }
    }
}