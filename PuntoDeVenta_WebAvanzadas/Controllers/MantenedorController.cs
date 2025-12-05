using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;

namespace PuntoDeVenta_WebAvanzadas.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }

        #region categoria
        //---------------------CATEGORIA-----------------------------

        [HttpGet]//significa que esto de abajo te devuelve datos, no es necesario mandarle datos en dado caso de quererlo
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = new CN_Categoria().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.CAT_ID == 0)
            {
                resultado = new CN_Categoria().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region marca
        //---------------------MARCA-----------------------------

        [HttpGet]//significa que esto de abajo te devuelve datos, no es necesario mandarle datos en dado caso de quererlo
        public JsonResult ListarMarcas()
        {
            List<Marca> oLista = new List<Marca>();
            oLista = new CN_Marca().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.MAR_ID == 0)
            {
                resultado = new CN_Marca().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Marca().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region producto
        //---------------------PRODUCTO----------------------------

        [HttpGet]//significa que esto de abajo te devuelve datos, no es necesario mandarle datos en dado caso de quererlo
        public JsonResult ListarProductos()
        {
            List<Producto> oLista = new List<Producto>();
            oLista = new CN_Producto().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {

            string mensaje = string.Empty;
            bool operacionExitosa = true;
            bool guardarImagen_exito = true;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            decimal precio;
            if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint,new CultureInfo("es-MX"),out precio))
            {
                oProducto.PROD_PRECIO = precio;
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.##" },JsonRequestBehavior.AllowGet);
            }

            if (oProducto.PROD_ID == 0)
            {
                int IdProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);
                if(IdProductoGenerado != 0)
                {
                    oProducto.PROD_ID = IdProductoGenerado;
                }
                else
                {
                    operacionExitosa = false;
                }
            }
            else
            {
                operacionExitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }

            if(operacionExitosa == true)
            {
                if(archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];//para que se guarden las fotos ahi mandando llamar el servidor de webconfig
                    string extension = Path.GetExtension(archivoImagen.FileName);//obtener extension
                    string nombreImagen = string.Concat(oProducto.PROD_ID.ToString(),extension);//darle el mismo nombre que el id

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombreImagen));
                    }
                    catch(Exception ex)
                    {
                        string msg = ex.Message;
                        guardarImagen_exito = false;
                    }

                    if(guardarImagen_exito == true)
                    {
                        oProducto.PROD_IMAGEN = ruta_guardar;
                        oProducto.PROD_NOMBREIMAGEN = nombreImagen;
                        bool rspta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto pero hubo problemas con la imagen.";
                    }
                }
            }


            return Json(new { operacionExitosa = operacionExitosa,idGenerado = oProducto.PROD_ID, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oProducto = new CN_Producto().Listar().Where(p => p.PROD_ID == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.PROD_IMAGEN, oProducto.PROD_NOMBREIMAGEN), out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oProducto.PROD_NOMBREIMAGEN)
            },
            JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Producto().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        #endregion
    }
}