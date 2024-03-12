using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using System.Net;
using System.IO;


namespace CapaNegocio
{
    public class CN_Recursos
    {

        //Crear metodo que le genere una clave automática a su correo
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0,8);   //genera un guid que es una clave por parte de c#  - una clave de 0 a 8 digitos
            return clave;
        }



        //encriptar de Texto a SHA256  -> Para la clave

        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            //usando las referencias de "system.security.cryptography
            using(SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach(byte b in result)
                    Sb.Append(b.ToString("x2")); //la cadena sea x2 más grande
            }
            return Sb.ToString();
        }


        //crear el metodo para enviar la clave autogenerada hacia el correo gmail

        public static bool EnviarCorreo( string correo, string asunto, string mensaje)
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("omaraguilar899@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("omaraguilar899@gmail.com", "ttcrlmrdfymkxynq"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                resultado = true;

            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }



        //crear metodo de cadena de texto a base64

        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                conversion = false;
            }

            return textoBase64;
        }
    }
}
