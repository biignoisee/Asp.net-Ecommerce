using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//Agregamos las referencias
using CapaNegocio;
using CapaEntidad;
using Newtonsoft.Json;
using System.Globalization;
using System.Configuration;
using System.IO;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class MantenimientoController : Controller
    {
        // Trabajar con el Controlador de Categorias
        public ActionResult Categorias()
        {
            return View();
        }


        [HttpGet]
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

            if (objeto.IdCategoria == 0)
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
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }








        // Trabajar con el Controlador de Marcas
        public ActionResult Marcas()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ListarMarcas()
        {
            List<Marca> oLista = new List<Marca>();

            oLista = new CN_Marca().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GuardaMarca(Marca objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdMarca == 0)
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
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Marca().Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }






        // TRABAJAR CON CONTROLADOR PRODUCTOS


        public ActionResult Productos()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ListarProductos()
        {
            List<Producto> oLista = new List<Producto>();

            oLista = new CN_Producto().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GuardaProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;

            //trabajar con el guardado de imagenes
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;


            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            decimal precio;

            if(decimal.TryParse(oProducto.PrecioTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out precio))
            {
                oProducto.Precio = precio;
            } else
            {
                return Json(new { operacion_exitosa = false, mensaje ="El formato del precio debe ser ##.##"}, JsonRequestBehavior.AllowGet);
            }


            if (oProducto.IdProducto == 0)
            {
                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);

                if(idProductoGenerado != 0)
                {
                    oProducto.IdProducto = idProductoGenerado;
                } else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }



            //Registrar la imagen
            if (operacion_exitosa)
            {
                if(archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);

                    string nombre_imagen = string.Concat(oProducto.IdProducto.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    } 
                    catch(Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }


                    if (guardar_imagen_exito)
                    {
                        oProducto.RutaImagen = ruta_guardar;
                        oProducto.NombreImagen = nombre_imagen;
                        bool respuesta = new CN_Producto().GuardarDatosImagenes(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se ha guardado el producto, pero existe problemas con la imagen - revise la ruta en el webConfig (en nuestro servidor si funciona)";
                    }

                }
            }

            return Json(new { operacion_exitosa = operacion_exitosa,idGenerado = oProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oProducto.NombreImagen)
            },
                JsonRequestBehavior.AllowGet
            );
        }


        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Producto().Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        //fin mantenedor
    }
}