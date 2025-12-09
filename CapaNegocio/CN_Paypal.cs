using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; // <--- Para 'Task<>' y 'async'

using System.Configuration; // <--- Para leer Web.config
using System.Net;
using System.Net.Http;    // <--- Para hacer peticiones a PayPal
using System.Net.Http.Headers;

using Newtonsoft.Json;      // <--- Para leer la respuesta JSON
using Newtonsoft.Json.Linq;

namespace CapaNegocio
{
    public class CN_Paypal
    {
        private static string UrlPaypal = ConfigurationManager.AppSettings["PaypalUrl"];
        private static string ClientId = ConfigurationManager.AppSettings["PaypalClientId"];
        private static string Secret = ConfigurationManager.AppSettings["PaypalSecret"];

        public async Task<string> ObtenerToken()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlPaypal);
                var authToken = Encoding.ASCII.GetBytes($"{ClientId}:{Secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await client.PostAsync("/v1/oauth2/token", content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(jsonResult);
                    return json["access_token"].ToString();
                }
                return "";
            }
        }

        // Cambiamos el retorno a Task<string> (Devuelve la URL o vacío si falla)
        // Cambiamos 'Task<bool>' por 'Task<string>' y quitamos el 'out'
        public async Task<string> CrearOrden(string total, string moneda, string descripcion, string idTransaccion)
        {
            string urlAprobacion = "";

            try
            {
                string token = await ObtenerToken();
                if (string.IsNullOrEmpty(token)) return ""; // Si falla, retorna vacío

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UrlPaypal);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var orden = new
                    {
                        intent = "CAPTURE",
                        purchase_units = new[]
                        {
                    new {
                        amount = new {
                            currency_code = moneda,
                            value = total
                        },
                        description = descripcion,
                        reference_id = idTransaccion
                    }
                },
                        application_context = new
                        {
                            brand_name = "Mi Tienda",
                            landing_page = "NO_PREFERENCE",
                            user_action = "PAY_NOW",
                            return_url = "https://localhost:44358/Tienda/PagoEfectuado", // ¡VERIFICA TU PUERTO!
                            cancel_url = "https://localhost:44358/Tienda/Carrito"
                        }
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(orden), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("/v2/checkout/orders", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        JObject json = JObject.Parse(jsonResult);

                        foreach (var link in json["links"])
                        {
                            if (link["rel"].ToString() == "approve")
                            {
                                urlAprobacion = link["href"].ToString();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                urlAprobacion = ex.Message;
            }

            return urlAprobacion; // Devolvemos la URL directamente
        }
    }
}
