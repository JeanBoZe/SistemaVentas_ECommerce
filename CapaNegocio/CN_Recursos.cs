using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace CapaNegocio
{
    public class CN_Recursos
    {

        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);// metodo que genera codigo alfanumerico unico

            return clave;
        }

        // encriptacion de texto en SHA256
        public static string Encriptar(string texto)
        {
            StringBuilder sb = new StringBuilder();

            using(SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach(byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }

        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);// para quien va el mensaje
                mail.From = new MailAddress("jeanbojo07@gmail.com"); //quien va a enviar los correos
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()//servidor cliente que envia el correo
                {
                    Credentials = new NetworkCredential("jeanbojo07@gmail.com", "vjdzhlkkmkmjekyq"),
                    Host = "smtp.gmail.com", //servidor que usa gmail
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                resultado = true;
            }
            catch(Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }
        
        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch(Exception ex)
            {
                conversion = false;
            }

            return textoBase64;
        }
    }
}
