using MvcApiPersonajesAWS.Models;
using System.Net.Http.Headers;

namespace MvcApiPersonajesAWS.Services
{
    public class ServiceApiPersonajes
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceApiPersonajes(IConfiguration configuration)
        {
            this.UrlApi = configuration.GetValue<string>("ApiUrls:ApiPersonaje");
            this.Header = new MediaTypeWithQualityHeaderValue("applicatio/json");
        }


        public async Task<string> TestApiAsync()
        {
            string request = "/api/Personajes/";
            //utilizamos un manejador para la peticion de httpclient
            var handler = new HttpClientHandler();
            //indicamos al manejador como se comportatara al recibir peticiones
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = new Uri(this.UrlApi);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(this.Header);
            HttpResponseMessage response = await client.GetAsync(request);
            return "Respuesta " + response.StatusCode;
        }


        private async Task<T> CallApiAsync<T>
       (string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            string request = "/api/Personajes";
            List<Personaje> personajes = await this.CallApiAsync<List<Personaje>>(request);
            return personajes;
        }
    }
}
