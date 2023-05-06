using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Movie.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieDbController : ControllerBase
    {
        // GET: api/<TestController>
        [HttpGet]
        public async Task<string> Get()
        {

            string route = "https://api.themoviedb.org/3/movie/" + 551 + "?api_key=4f903b108c7a754673ed6a616b15889a";


            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)

            using (var client = new HttpClient(clientHandler))
            using (var httpRequest = new HttpRequestMessage())
            {
                // Build the request.
                httpRequest.Method = HttpMethod.Get;
                httpRequest.RequestUri = new Uri(route);


                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(httpRequest).ConfigureAwait(false);
                // Read response as a string.
                var result = await response.Content.ReadAsStringAsync(); // middleware apiden dönecek ve sonucu burda göstereceğiz

                if (result != null && !String.IsNullOrEmpty(result))
                {
                    var res = JsonConvert.DeserializeObject<Movie.Core.Models.Movie>(result);
                }
            }
            return "OK";
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}

// Gelen datalar json olarak geldiği için class objesine çevirilecek.
// Gelen class add işlemiyle database'e kaydet.

