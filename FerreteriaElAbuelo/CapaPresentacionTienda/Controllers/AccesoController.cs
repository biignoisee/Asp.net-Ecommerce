using CapaEntidad;
using CapaNegocio;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // Login
        public ActionResult Index()
        {
            return View();
        }

        //Registro Cliente
        public ActionResult RegistroCliente()
        {
            return View();
        }

        //Reestablecer Clave
        public ActionResult Reestablecer()
        {
            return View();
        }

        //Cambiar Clave
        public ActionResult CambioClave()
        {
            return View();
        }


        [HttpPost]
        public ActionResult RegistroCliente(Cliente objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;
            
            if(objeto.Clave != objeto.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            resultado = new CN_Cliente().Registrar(objeto, out mensaje);
            if(resultado > 0)
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
        public ActionResult Index(string correo, string clave)
        {
            Cliente oCliente = null;

            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo && item.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if(oCliente == null)
            {
                ViewBag.Error = "Correo o Contraseña no son correctas";
                return View();
            }
            else
            {
                if (oCliente.Reestablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambioClave", "Acceso");
                }else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo,false);

                    Session["Cliente"] = oCliente;

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }

        }



        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "No se encontro un cliente con ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().ReestablacerClave(oCliente.IdCliente, correo, out mensaje);

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
        public ActionResult CambioClave(string idCliente, string claveActual, string nuevaClave, string confirmarClave)
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(u => u.IdCliente == int.Parse(idCliente)).FirstOrDefault();

            //validar si la clave entrante es la correcta o no?

            if (oCliente.Clave != CN_Recursos.ConvertirSha256(claveActual))
            {
                TempData["IdCliente"] = idCliente;

                ViewData["vClave"] = "";

                ViewBag.Error = "La contraseña actual no es la correcta";
                return View();
            }
            else if (nuevaClave != confirmarClave)
            {
                TempData["IdCliente"] = idCliente;

                ViewData["vClave"] = claveActual;

                ViewBag.Error = "La contraseñas no coinciden";
                return View();
            }

            ViewData["vClave"] = "";

            nuevaClave = CN_Recursos.ConvertirSha256(nuevaClave);
            string mensaje = string.Empty;

            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idCliente), nuevaClave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = idCliente;
                ViewBag.Error = "mensaje";
                return View();
            }
        }





        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Tienda");
        }


    }
}