using MvcApiPersonajesAWS.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

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
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                //indicamos al manejador como se comportatara al recibir peticiones
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };

                using (HttpClient client = new HttpClient(handler))
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

        }

        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            string request = "/api/Personajes";
            List<Personaje> personajes = await this.CallApiAsync<List<Personaje>>(request);
            return personajes;
        }


        public async Task InsertPersonajeProcedure(string nombre, string imagen)
        {
            
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Procedure";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                //tenemos que enviar un objeto JSON
                //nos creamos un objeto de la clase Hospital
                Personaje personaje = new Personaje
                {
                    Nombre = nombre,
                    Imagen = imagen

                };
                //convertimos el objeto a json
                string json = JsonConvert.SerializeObject(personaje);
                //para enviar datos al servicio se utiliza 
                //la clase SytringContent, donde debemos indicar
                //los datos, de ending  y su tipo
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
            }
        }

        public async Task UpdatePersonajeProcedure(int id, string nombre, string imagen)
        {

            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Procedure";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                //tenemos que enviar un objeto JSON
                //nos creamos un objeto de la clase Hospital
                Personaje personaje = new Personaje
                {
                    IdPersonaje = id,
                    Nombre = nombre,
                    Imagen = imagen

                };
                //convertimos el objeto a json
                string json = JsonConvert.SerializeObject(personaje);
                //para enviar datos al servicio se utiliza 
                //la clase SytringContent, donde debemos indicar
                //los datos, de ending  y su tipo
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(request, content);
            }
        }

        public async Task DeletePersonaje(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Procedure/" + id;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage response = await client.DeleteAsync(request);
            }
        }

    }
}
