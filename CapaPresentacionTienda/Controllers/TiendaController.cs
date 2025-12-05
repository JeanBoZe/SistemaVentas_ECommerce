using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO; // Necesario para manejar rutas de archivos

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // ============================================
        // 1. VISTA PRINCIPAL (CATÁLOGO)
        // ============================================
        public ActionResult Index()
        {
            return View();
        }

        // ============================================
        // 2. VISTA DETALLE (CON IMAGEN Y COMENTARIOS)
        // ============================================
        public ActionResult DetalleProducto(int idproducto = 0)
        {
            if (idproducto == 0) return RedirectToAction("Index");

            Producto oProducto = new CN_Producto().Listar().Where(p => p.PROD_ID == idproducto).FirstOrDefault();

            if (oProducto != null)
            {
                oProducto.oListaComentarios = new CN_Comentario().Listar(idproducto);

                // LÓGICA DE IMAGEN CORREGIDA
                bool conversion;
                string rutaImagen = "";

                if (!string.IsNullOrEmpty(oProducto.PROD_IMAGEN))
                {
                    rutaImagen = Path.Combine(oProducto.PROD_IMAGEN, oProducto.PROD_NOMBREIMAGEN);
                }
                else
                {
                    rutaImagen = ObtenerRutaImagen(oProducto.PROD_NOMBREIMAGEN);
                }

                oProducto.Base64 = CN_Recursos.ConvertirBase64(rutaImagen, out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.PROD_NOMBREIMAGEN).Replace(".", "");
            }

            return View(oProducto);
        }

        // ============================================
        // 3. MÉTODOS PARA FILTROS (RESTAURADOS)
        // ============================================
        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new CN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        // ¡ESTE ES EL QUE FALTABA Y DABA ERROR 404!
        [HttpPost]
        public JsonResult ListaMarcasPorCategoria(int idcategoria)
        {
            List<Marca> lista = new CN_Marca().ListarMarcaPorCategoria(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        // ============================================
        // 4. LISTAR PRODUCTOS (CONVERTIR IMÁGENES)
        // ============================================
        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            List<Producto> lista = new CN_Producto().Listar();

            // Filtros
            if (idcategoria > 0)
                lista = lista.Where(p => p.oCategoria.CAT_ID == idcategoria).ToList();

            if (idmarca > 0)
                lista = lista.Where(p => p.oMarca.MAR_ID == idmarca).ToList();

            // Dentro de [HttpPost] ListarProducto...

            // ... filtros de categoria y marca (igual que antes) ...

            foreach (Producto item in lista)
            {
                bool conversion;
                string rutaImagen = "";

                try
                {
                    // ESTRATEGIA: Usar la ruta que ya viene de la base de datos
                    // Tu captura muestra que PROD_IMAGEN ya tiene "D:\Universidad\...\Images_Producto"
                    if (!string.IsNullOrEmpty(item.PROD_IMAGEN))
                    {
                        rutaImagen = Path.Combine(item.PROD_IMAGEN, item.PROD_NOMBREIMAGEN);
                    }
                    else
                    {
                        // Si la BD no trae ruta, usamos el cálculo manual como respaldo
                        rutaImagen = ObtenerRutaImagen(item.PROD_NOMBREIMAGEN);
                    }

                    // TRUCO DE DEBUG: 
                    // Si la imagen sale rota, la "Descripción" del producto te dirá qué ruta intentó leer.
                    // (Borra esta línea cuando ya funcionen las imágenes)
                    // item.PROD_DESCRIPCION = "Ruta: " + rutaImagen; 

                    // Conversión usando tu clase CN_Recursos
                    item.Base64 = CN_Recursos.ConvertirBase64(rutaImagen, out conversion);
                    item.Extension = Path.GetExtension(item.PROD_NOMBREIMAGEN).Replace(".", "");
                }
                catch
                {
                    continue;
                }
            }

            // ... retorno del Json (igual que antes) ...

            var jsonResult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue; // Permite enviar muchas fotos

            return jsonResult;
        }

        // ============================================
        // 5. HELPER PARA CARPETA EXTERNA
        // ============================================
        private string ObtenerRutaImagen(string nombreImagen)
        {
            if (string.IsNullOrEmpty(nombreImagen)) return "";

            // Buscamos la carpeta "Images_Producto" que está FUERA del proyecto web
            string rutaTienda = Server.MapPath("~");
            string rutaSolucion = Directory.GetParent(rutaTienda).FullName;
            return Path.Combine(rutaSolucion, "Images_Producto", nombreImagen);
        }

        // ============================================
        // 6. CARRITO Y COMENTARIOS
        // ============================================
        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            if (Session["Cliente"] == null) return Json(new { respuesta = false, mensaje = "Inicia sesión" });

            int idcliente = ((Cliente)Session["Cliente"]).CLI_ID;
            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);
            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe) mensaje = "El producto ya existe en el carrito";
            else respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarComentario(string contenido, int puntuacion, int idproducto)
        {
            bool resultado = false;
            string mensaje = string.Empty;
            if (Session["Cliente"] == null) return Json(new { resultado = false, mensaje = "Debes iniciar sesión" });

            Cliente oCliente = (Cliente)Session["Cliente"];
            Comentario oComentario = new Comentario()
            {
                COM_PROD_ID = idproducto,
                COM_CLI_ID = oCliente.CLI_ID,
                COM_PUNTUACION = puntuacion,
                COM_CONTENIDO = contenido
            };
            resultado = new CN_Comentario().Registrar(oComentario, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        public ActionResult Carrito()
        {
            return View();
        }
    }
}