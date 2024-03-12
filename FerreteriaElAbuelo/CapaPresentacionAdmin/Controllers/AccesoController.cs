using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;


//autenticacion por formularios, que nadie entre por las url
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        

        public ActionResult CambiarClave()
        {
            return View();
        }


        public ActionResult ReestablecerClave()
        {
            return View();
        }


        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if(oUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña no es la correcta";  //viebag en la misma vista
                return View();
            } 
            else
            {
                //crearemos validaciones que lo redirija al usuario a reestablecer la contraseña o a ingresar al sistema
                //en su base de datos el campo restablecer estara activo 1
                if (oUsuario.Reestablecer)
                {
                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }

                //autenticacion por medio del correo
                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);


                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");

            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idUsuario, string claveActual, string nuevaClave, string confirmarClave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idUsuario)).FirstOrDefault();

            //validar si la clave entrante es la correcta o no?

            if(oUsuario.Clave != CN_Recursos.ConvertirSha256(claveActual))
            {
                TempData["IdUsuario"] = idUsuario;

                ViewData["vClave"] = "";

                ViewBag.Error = "La contraseña actual no es la correcta";
                return View();
            } 
            else if(nuevaClave != confirmarClave)
            {
                TempData["IdUsuario"] = idUsuario;

                ViewData["vClave"] = claveActual;

                ViewBag.Error = "La contraseñas no coinciden";
                return View();
            }

            ViewData["vClave"] = "";

            nuevaClave = CN_Recursos.ConvertirSha256(nuevaClave);
            string mensaje = string.Empty;

            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idUsuario), nuevaClave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            } 
            else
            {
                TempData["IdUsuario"] = idUsuario;
                ViewBag.Error = "mensaje";
                return View();
            }
        }


        [HttpPost]
        public ActionResult ReestablecerClave(string correo)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if(oUsuario == null)
            {
                ViewBag.Error = "No se encontro un usuario con ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().ReestablacerClave(oUsuario.IdUsuario, correo, out mensaje);

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

        //cerrar sesion
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }

    }
}