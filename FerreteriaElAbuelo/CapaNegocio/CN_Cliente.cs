using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


//Agregamos las respectivas referencias
using CapaDatos;
using CapaEntidad;


namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objCapaDato = new CD_Cliente();

        //Capa en donde aplicamos todas las reglas del negocio
        public List<Cliente> Listar()
        {
            return objCapaDato.Listar();
        }



        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "Nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "Apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio, ingresa un nombre";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                obj.Clave = CN_Recursos.ConvertirSha256(obj.Clave);
                return objCapaDato.Registrar(obj, out Mensaje);

            }
            else
            {
                return 0;
            }
        }


        //Metodo para trabajar con la clave
        public bool CambiarClave(int idCliente, string nuevaClave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idCliente, nuevaClave, out Mensaje);
        }



        //Reestablecer clave

        public bool ReestablacerClave(int idCliente, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaClave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerClave(idCliente, CN_Recursos.ConvertirSha256(nuevaClave), out Mensaje);

            if (resultado)
            {
                string asunto = "Contraseña Reestablecida";
                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaClave);
                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar ningún correo";
                    return false;
                }

            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }

        }
    }
}
