using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//Agregamos las referencias
using CapaNegocio;
using CapaEntidad;
using System.Web.Services.Description;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oLista = new List<Usuario>();

            oLista = new CN_Usuarios().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GuardarUsuario(Usuario objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdUsuario == 0)
            {
                resultado = new CN_Usuarios().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Usuarios().Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult VistaDashboard()
        {
            Dashboard objeto = new CN_Reporte().VerDashboard();
            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListaReporte(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();

            oLista = new CN_Reporte().Ventas( fechaInicio, fechaFin, idTransaccion);

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public FileResult ExportarVenta(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Reporte().Ventas(fechaInicio, fechaFin, idTransaccion);

            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("es-PE");
            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente Venta", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));


            foreach(Reporte rp in oLista)
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }

            dt.TableName = "Datos";

            using(XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte Venta " + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }
    }
}